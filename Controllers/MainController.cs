using Microsoft.AspNetCore.Mvc;

namespace NationalDisplay.Controllers;

public class MainController: Controller
{
    public IActionResult ManageAccount()
    {
        
        return RedirectToAction("ManageAccountRedirect", "Main");
        // return View("/views/home/monitor/manageaccount.cshtml");
    }

    public IActionResult ManageAccountRedirect()
    {
        if(!AccountController.isLogin){
            return NoContent();
        }
        return View("/views/home/monitor/manageaccount.cshtml");
    }

    public IActionResult Display()
    {
            PlanListController p = new PlanListController();
            return p.Index();
    }
    public IActionResult Index()
    {
        if(!AccountController.isLogin){
            return NoContent();
        }
        return View("/views/home/monitor/main.cshtml");
    }

    public IActionResult Login()
    {
        AccountController.isLogin = false;
        Console.WriteLine("Login page");
        return View("/views/home/monitor/Login.cshtml");
    }
}