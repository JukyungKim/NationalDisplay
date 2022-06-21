using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;
using Microsoft.AspNetCore.SignalR;
using System.Web;

namespace NationalDisplay.Controllers;

public class LoginController : Controller
{
    static public int indexTest = 0;
    [Route("home/login")]
    public IActionResult MonitorPlan()
    {
        Console.WriteLine("Monitor plan");

        return View("/views/home/monitor/login.cshtml");
    }

    // public IActionResult Start()
    // {
    //     return View("/")
    // }
}