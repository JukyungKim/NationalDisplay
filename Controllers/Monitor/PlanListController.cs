using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;

namespace NationalDisplay.Controllers;

public class PlanListController : Controller
{
    static public List<string[]> planListBuf = new List<string[]>();
 
    public Plan plan = new Plan();

    private PlanListModel planListModel = new PlanListModel();
    // [Route("home/planlist")]
    public IActionResult Index()
    {
        Console.WriteLine("Plan list");

        planListBuf.Clear();
        planListModel.LoadPlanListInfo();
        while(!PipeClient.sendComplete);
        plan.planList = planListBuf;

        return View("/views/home/monitor/planlist.cshtml", plan);
        // return NoContent();
    }

    public void GetSensorPosition(string firstName, string lastName)
    {
        Console.WriteLine("Get Sensor : {0}, {1}", firstName, lastName);
    }

    public IActionResult LoadPlan(string id)
    {
        Console.WriteLine("Plan id : " + id);

        SensorTask.planId = id;

        PlanListModel planList = new PlanListModel();
        PlanModel planModel = planList.LoadPlanImage(id);



        while(!PipeClient.sendComplete);

        return View("views/home/monitor/plan.cshtml", PlanController.sensorList);
    }

    public IActionResult LoadPlanList()
    {        

        plan.planList = planListBuf;
        foreach(var p in planListBuf){
            Console.WriteLine("aa:" + p[0]);
        }

        return View("/views/home/monitor/planlist.cshtml", plan);
    }
}

public class Plan{
    public List<string[]> planList = new List<string[]>(); 
}
