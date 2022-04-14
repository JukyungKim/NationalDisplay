using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Controllers;
using NationalDisplay.Models;

namespace NationalDisplay.Models;
public class PipeClient
{
    private static int numClients = 4;
    public static int command = 4;
    public static int sensorId = 0;
    public static string planId = "";

    public delegate IActionResult LoadPlanList();
    static public LoadPlanList? loadPlanList;
    static public bool sendComplete = false;
    public static async void Start()
    {
        sendComplete = false;
        Console.WriteLine("Pipe client starts");
        await ClientTask();
    }
    public static Task ClientTask()
    {
        Action<object> action = obj =>
        {
            string[] args = { "spawnclient" };

            if (args.Length > 0)
            {
                if (args[0] == "spawnclient")
                {
                    var pipeClient =
                        new NamedPipeClientStream(".", "testpipe",
                            PipeDirection.InOut, PipeOptions.None,
                            TokenImpersonationLevel.Impersonation);

                    Console.WriteLine("Connecting to server...\n");
                    pipeClient.Connect();

                    var ss = new StreamString(pipeClient);
                    // Validate the server's signature string.
                    if (ss.ReadString() == "I am the one true server!")
                    {
                        // The client security token is sent with the first write.
                        // Send the name of the file whose contents are returned
                        // by the server.
                        // ss.WriteString("c:\\textfile.txt");

                        List<byte> data = new List<byte>();
                        if(command == 1)    PipeProtocol.sendSensorInfo(planId, data);
                        else if(command == 2) PipeProtocol.sendSensorData(planId, data);
                        else if(command == 3) PipeProtocol.sendPlanImage(planId, data);
                        else if(command == 4) PipeProtocol.sendPlanInfo(data);
                        ss.WriteByte(data);
                        PipeProtocol.receivedData(ss.ReadByte());
                        sendComplete = true;
                        // // Print the file to the screen.
                        // Console.WriteLine(ss.ReadString());

                        Console.WriteLine("Complete send data");
                    }
                    else
                    {
                        Console.WriteLine("Server could not be verified.");
                    }

                    pipeClient.Close();
                    // Give the client process some time to display results before exiting.
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("\n*** Named pipe client stream with impersonation example ***\n");
                StartClients();
            }
        };

        Task task = new Task(action, "pipe_client");
        task.Start();

        return task;

    }

    // Helper function to create pipe client processes
    private static void StartClients()
    {
        string currentProcessName = Environment.CommandLine;

        // Remove extra characters when launched from Visual Studio
        currentProcessName = currentProcessName.Trim('"', ' ');

        currentProcessName = Path.ChangeExtension(currentProcessName, ".exe");
        Process[] plist = new Process[numClients];

        Console.WriteLine("Spawning client processes...\n");

        if (currentProcessName.Contains(Environment.CurrentDirectory))
        {
            currentProcessName = currentProcessName.Replace(Environment.CurrentDirectory, String.Empty);
        }

        // Remove extra characters when launched from Visual Studio
        currentProcessName = currentProcessName.Replace("\\", String.Empty);
        currentProcessName = currentProcessName.Replace("\"", String.Empty);

        int i;
        for (i = 0; i < numClients; i++)
        {
            // Start 'this' program but spawn a named pipe client.
            plist[i] = Process.Start(currentProcessName, "spawnclient");
        }
        while (i > 0)
        {
            for (int j = 0; j < numClients; j++)
            {
                if (plist[j] != null)
                {
                    if (plist[j].HasExited)
                    {
                        Console.WriteLine($"Client process[{plist[j].Id}] has exited.");
                        plist[j] = null;
                        i--;    // decrement the process watch count
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                }
            }
        }
        Console.WriteLine("\nClient processes finished, exiting.");
    }
}

// Defines the data protocol for reading and writing strings on our stream.
public class StreamString
{
    private Stream ioStream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len;
        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        var inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);
        Console.WriteLine(streamEncoding.GetString(inBuffer));

        return streamEncoding.GetString(inBuffer);
    }


    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }

    public byte[] ReadByte()
    {
        int len;
        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        var inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);
        // Console.Write("Received byte : ");
        // foreach(var d in inBuffer){
        //     Console.Write("{0} ", d);
        // }
        // Console.WriteLine("");
        // Console.WriteLine(Encoding.Default.GetString(inBuffer));
        return inBuffer;
    }

    public void WriteByte(List<byte> data)
    {
        byte[] buf = data.ToArray();
        int len = buf.Length;
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(buf, 0, len);
        ioStream.Flush();
    }
}