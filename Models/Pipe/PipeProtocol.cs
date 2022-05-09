
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Controllers;
using System.Drawing;

namespace NationalDisplay.Models;

public class PipeProtocol
{
    public enum EProtocol
    {
        SENSOR_INFO = 1, SENSOR_DATA, PLAN_IMAGE, PLAN_INFO
    };
    public static void selectProtocol(int protocol, List<byte> data)
    {
        // switch ((EProtocol)protocol)
        // {
        //     case EProtocol.SENSOR_INFO:
        //         sendSensorInfo(protocol, data);
        //         break;
        //     case EProtocol.SENSOR_DATA:
        //         sendSensorData(protocol, data);
        //         break;
        //     case EProtocol.PLAN_IMAGE:
        //         sendPlanImage(protocol, data);
        //         break;
        //     default:
        //         break;
        // }
    }
    public static void receivedData(byte[] data)
    {
        int command = data[0];

        Console.WriteLine("Received command : " + command);

        switch ((EProtocol)command)
        {
            case EProtocol.SENSOR_INFO:

                break;
            case EProtocol.SENSOR_DATA:
                receiveSensorData(data);
                break;
            case EProtocol.PLAN_IMAGE:
                receivePlanImage(data);
                break;
                
            case EProtocol.PLAN_INFO:
                receivePlanInfo(data);
                break;
            default:
                break;
        }
    }

    public static void receivePlanInfo(byte[] data)
    {
        List<byte[]> buf = new List<byte[]>();
        int dataBodySize = data.Length - 1;
        int DATA_BODY_START_POSITION = 1;
        int PACKET_SIZE = 30;
        int index = DATA_BODY_START_POSITION;

        Console.WriteLine("size" + data.Length);

        // byte[] -> List<byte[]> 변환
        while(index < data.Length){
            byte[] temp = new byte[30];
            for(int i = 0; i < PACKET_SIZE; i++){
                temp[i] = data[index];
                index++;
            }
            buf.Add(temp);
        }

        index = 0;
        string[] info = new string[6];

        // List<byte[]> -> List<string[]> 변환
        foreach(var b in buf){       
            info[index] = Encoding.Default.GetString(b);
            string temp3 = info[index];
            info[index] = "";

            foreach(var i in temp3){
                if(i != '\0'){
                    info[index] += i;
                }
            }
            index++;
            if(index >= 6){
                PlanListController.planListBuf.Add(info);
                info = new string[6];
                index = 0;
            }
        }
    }
    public static void receivePlanImage(byte[] data)
    {
        Console.WriteLine("Recieved plan image");

        byte[] productImageByte = new byte[data.Length - 1];
        string IMG_PATH = "images/plan.png";
    
        Array.Copy(data, 1, productImageByte, 0, data.Length - 1);

        if (productImageByte != null)
        {
            using (MemoryStream productImageStream = new System.IO.MemoryStream(productImageByte))
            {
                ImageConverter imageConverter = new System.Drawing.ImageConverter();

                // pictureBox1.Image = imageConverter.ConvertFrom(productImageByte) as System.Drawing.Image;
                Bitmap bitmap = new Bitmap(imageConverter.ConvertFrom(productImageByte) as System.Drawing.Image);
                bitmap.Save("wwwroot/" + IMG_PATH);
                Console.WriteLine("Image loaded");
            }
        }
    }

    public static void receiveSensorData(byte[] data)
    {
        Console.WriteLine("Received sensor data:" + data.Length);
        foreach(var d in data){
            Console.Write(d + " ");
        }
        Console.WriteLine();
        
        int sensorNumber = (data.Length - 1) / 18;

        for(int i = 0; i < sensorNumber; i++){
            int num = (18 * i) + 1; // 18 : id(6byte) + 연기,온도,가스(4,4,4)
            SensorData s = new SensorData();

            s.id = (ulong)((data[num]) | (data[num + 1] << 8) | (data[num + 2] << 16) 
                | (data[num + 3] << 24) | (data[num + 4] << 32) | (data[num + 5] << 40));
            num += 6;

            s.smoke = (int)((data[num]) | (data[num + 1] << 8) | (data[num + 2] << 16) 
                | (data[num + 3] << 24));
            num += 4;          

            s.temp = (int)((data[num]) | (data[num + 1] << 8) | (data[num + 2] << 16) 
                | (data[num + 3] << 24));
            num += 4;

            s.gas = (int)((data[num]) | (data[num + 1] << 8) | (data[num + 2] << 16) 
                | (data[num + 3] << 24));

            // 중복된 센서 ID 있으면 추가 안함
            bool existId = false;
            int index = 0;
            foreach(var sensor in PlanController.sensorList){
                if(sensor.id == s.id){
                    existId = true;
                    break;
                }
                index++;
            }
            if(!existId){
                PlanController.sensorList.Add(s);
            }
            else{
                PlanController.sensorList[index].smoke = s.smoke;
                PlanController.sensorList[index].temp = s.temp;
                PlanController.sensorList[index].gas = s.gas;
            }       
        }
    }
    public static void sendSensorInfo(string planId, List<byte> data)
    {
        data.Add((byte)EProtocol.SENSOR_INFO);
        foreach (var s in planId)
        {
            data.Add((byte)s);
        }
    }
    public static void sendSensorData(string planId, List<byte> data)
    {
        Console.WriteLine("Send sensor data : " + planId);
        data.Add((byte)EProtocol.SENSOR_DATA);
        byte[] planIdByte = Encoding.Default.GetBytes(planId);
        foreach(var v in planIdByte){
            data.Add(v);
        }
    }
    public static void sendPlanImage(string planId, List<byte> data)
    {
        Console.WriteLine("Send plan image");
        data.Add((byte)EProtocol.PLAN_IMAGE);
        byte[] planIdByte = Encoding.Default.GetBytes(planId);
        foreach(var v in planIdByte){
            data.Add(v);
        }
        // foreach (var s in planId)
        // {
        //     data.Add((byte)s);
        // }
    }

    public static void sendPlanInfo(List<byte> data)
    {
        data.Add((byte)EProtocol.PLAN_INFO);
    }
}