using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;
using Microsoft.AspNetCore.SignalR;
using System.Web;

namespace NationalDisplay.Controllers;

public class PlanController : Controller
{
    static public List<SensorData> sensorList = new List<SensorData>();
    static public int indexTest = 0;
    [Route("home/monitor")]
    public IActionResult MonitorPlan()
    {
        Console.WriteLine("Monitor plan");

        // SensorData s = new SensorData();
        // s.id = 1;
        // s.gas = 2;
        // s.smoke = 3;
        // s.temp = 4;
        // sensorList.sensorList.Add(s);

        return View("/views/home/monitor/plan.cshtml");
    }

    public void GetSensorPosition(string firstName, string lastName)
    {
        Console.WriteLine("Get Sensor : {0}, {1}", firstName, lastName);
    }
}

public class SensorHub: Hub
{
    public async Task SendSensorData(string u, string m)
    {
        // PlanController.sensorList = new List<SensorData>();
        // SensorData sensor = new SensorData();
        // sensor.id = 1;
        // sensor.smoke = 100 + PlanController.indexTest;
        // sensor.gas = 200 + PlanController.indexTest;
        // sensor.temp = 300 + PlanController.indexTest;
        // PlanController.sensorList.Add(sensor);

        // sensor = new SensorData();
        // sensor.id = 2;
        // sensor.smoke = 500 + PlanController.indexTest;;
        // sensor.gas = 600 + PlanController.indexTest;
        // sensor.temp = 700 + PlanController.indexTest;
        // PlanController.sensorList.Add(sensor);
        // PlanController.indexTest++;
 
        int index = 0;
        foreach(var s in PlanController.sensorList){
            List<string> list = new List<string>();

            list.Add(s.id.ToString());
            list.Add(s.smoke.ToString());
            list.Add(s.temp.ToString());
            list.Add(s.gas.ToString());

            int[] pos = LoadSensorPosition((int)s.id);
            list.Add(pos[0].ToString());
            list.Add(pos[1].ToString());

            await Clients.All.SendAsync("ReceiveSensorData", list, index);
            index++;
            
        }
    }

    public async Task SaveSensorPosition(int id, int x, int y)
    {
        byte[] buf = new byte[12];
        buf[0] = (byte)(id >> 24);
        buf[1] = (byte)(id >> 16);
        buf[2] = (byte)(id >> 8);
        buf[3] = (byte)(id >> 0);
        buf[4] = (byte)(x >> 24);
        buf[5] = (byte)(x >> 16);
        buf[6] = (byte)(x >> 8);
        buf[7] = (byte)(x >> 0);
        buf[8] = (byte)(y >> 24);
        buf[9] = (byte)(y >> 16);
        buf[10] = (byte)(y >> 8);
        buf[11] = (byte)(y >> 0);
        string folderPath = "wwwroot/sensor_pos/";
        DirectoryInfo di = new DirectoryInfo(folderPath);
        if(!di.Exists){
            di.Create();
        }
        FileStream fs = new FileStream(String.Format("wwwroot/sensor_pos/sensor{0}", id), FileMode.OpenOrCreate, FileAccess.Write);
        fs.Write(buf, 0, 12);
        fs.Close();
        Console.WriteLine("SaveSensorPosition:{0}, {1}, {2}", id, x, y);
    }

    public int[] LoadSensorPosition(int id)
    {
        int[] pos = new int[2];

        string folderPath = "wwwroot/sensor_pos/";
        string filePath = String.Format("wwwroot/sensor_pos/sensor{0}", id);
        DirectoryInfo di = new DirectoryInfo(folderPath);
        if(!di.Exists || !File.Exists(filePath)){
            pos[0] = 100;
            pos[1] = 100;
            return pos;
        }
        FileStream fs = new FileStream(filePath, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] buf = br.ReadBytes(12);

        pos[0] = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | (buf[7] << 0);
        pos[1] = (buf[8] << 24) | (buf[9] << 16) | (buf[10] << 8) | (buf[11] << 0);

        // Console.WriteLine("LoadSensorPosition:{0} {1}", pos[0], pos[1]);
        fs.Close();
        return pos;
    }
}

public class ChatHub: Hub
{
    public async Task SendMessage(string user, string message)
    {
        List<string> list = new List<string>();
        Console.WriteLine("SignalR send");
        string temp = "test";
        // PlanController.indexTest++;
        list.Add("a");
        list.Add("b");
        foreach(var e in list){
            
            temp = PlanController.indexTest.ToString();
            await Clients.All.SendAsync("ReceiveMessage", e, temp);
            PlanController.indexTest++;
        }
        // await Clients.All.SendAsync("ReceiveMessage", list, temp);
    }
}
