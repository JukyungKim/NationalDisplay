using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;
using Microsoft.AspNetCore.SignalR;
using System.Web;

namespace NationalDisplay.Controllers;

public class PlanController : Controller
{
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

public class ChatHub: Hub
{
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine("SignalR send");
        string temp = "test";
        PlanController.indexTest++;
        await Clients.All.SendAsync("ReceiveMessage", PlanController.indexTest.ToString(), temp);
    }
}

public class SensorList{
    public List<SensorData> sensorList = new List<SensorData>();
}
