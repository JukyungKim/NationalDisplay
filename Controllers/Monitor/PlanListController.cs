using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;

namespace NationalDisplay.Controllers;

public class PlanListController : Controller
{
    private PlanListModel planListModel = new PlanListModel();
    [Route("home/planlist")]
    public IActionResult Index()
    {
        Console.WriteLine("Plan list");
        planListModel.LoadPlanListInfo();
        return View("/views/home/monitor/planlist.cshtml");
    }

    public void GetSensorPosition(string firstName, string lastName)
    {
        Console.WriteLine("Get Sensor : {0}, {1}", firstName, lastName);
    }

    public IActionResult LoadPlan(string id)
    {
        Console.WriteLine("Plan id : " + id);

        PlanListModel planList = new PlanListModel();
        PlanModel planModel = planList.LoadPlanImage(id);

        return View("views/home/monitor/plan.cshtml", planModel);
    }
}
