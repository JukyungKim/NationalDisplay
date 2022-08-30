using Microsoft.AspNetCore.Mvc;

namespace NationalDisplay.Controllers;

public class MainController: Controller
{
    public IActionResult ManageAccount()
    {
        

        return View("/views/home/monitor/manageaccount.cshtml");
    }

    public IActionResult Display()
    {
            PlanListController p = new PlanListController();
            return p.Index();
    }
}