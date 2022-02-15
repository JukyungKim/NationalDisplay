using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;

namespace NationalDisplay.Controllers;

public class PlanController : Controller
{
    [Route("home/monitor")]
    public IActionResult MonitorPlan()
    {
        Console.WriteLine("Monitor plan");
        return View("/views/home/monitor/plan.cshtml");
    }

    public void GetSensorPosition(string firstName, string lastName)
    {
        Console.WriteLine("Get Sensor : {0}, {1}", firstName, lastName);
    }
}
