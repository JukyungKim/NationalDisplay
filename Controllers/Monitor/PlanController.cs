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

            await Clients.All.SendAsync("ReceiveSensorData", list, index);
            index++;
            
        }
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
