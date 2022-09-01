using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using NationalDisplay.Models;

namespace NationalDisplay.Controllers;

public class AccountController: Controller
{
    private readonly ILogger<HomeController> _logger;
    public static string account_id = "";

    public AccountController(ILogger<HomeController> logger)
    {
        _logger = logger;
 
    }

    [Route("home/login")]
    public IActionResult Login()
    {
        Console.WriteLine("Login page");
        return View("/views/home/monitor/login.cshtml");
    }

    public IActionResult CheckLogin(string id, string password)
    {
        Console.WriteLine("Check login : {0} {1}", id, password);


        int ok;
        ok = AccountModel.CheckAccount(id, password);
        AccountModel.SaveLogInfo(id, ok);
        account_id = id;

        if(ok == 0){
            Console.WriteLine("Password wrong");
            return NoContent();
        }
        else if(ok == 1){
            return View("views/home/monitor/main.cshtml");
            /*
            PlanListController p = new PlanListController();
            return p.Index();
            // return View("/views/home/monitor/planlist.cshtml");
            */
        }
        else{
            Console.WriteLine("Password not exist");
            return NoContent();
        }
    }

    public IActionResult ChangePasswordPage()
    {
        return View("/views/home/monitor/password.cshtml");
    }

    public IActionResult ChangePassword(string password0, string password1, string password2)
    {
        var result = AccountModel.CheckAccount("master", password0);

        Console.WriteLine("Change password");
        bool hasChar = false;
        bool hasNum = false;

        if(string.IsNullOrEmpty(password1)){
            Console.WriteLine("Empty password");
            return NoContent();
        }

        string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialChArray = specialCh.ToCharArray();
        foreach (char ch in specialChArray) {
            if (password1.Contains(ch))
                hasChar = true;
        }

        string num = @"1234567890";
        char[] numArray = num.ToCharArray();
        foreach (char ch in numArray) {
            if (password1.Contains(ch))
                hasNum = true;
        }
        
        if (password1.Contains(" ")){
            Console.WriteLine("Exist space");
            return NoContent();
        }
        else if(password1.Length < 10 || password1.Length > 20){
            Console.WriteLine("Not length");
            return NoContent();
        }
        else if(!password1.Any(char.IsUpper)){
            Console.WriteLine("Not upper");
            return NoContent();
        }
        else if(!password1.Any(char.IsLower)){
            Console.WriteLine("Not lower");
            return NoContent();
        }
        else if(!hasChar){
            Console.WriteLine("Not char");
            return NoContent();
        }
        else if(!hasNum){
            Console.WriteLine("Not num");
            return NoContent();
        }
        else if(password1 != password2){
            Console.WriteLine("Not same password");
            return NoContent();
        }
        else if(result == 0){
            Console.WriteLine("Invalid password");
            return NoContent();
        }
        else{
            AccountModel.UpdatePassword("master", password1);
            return View("/views/home/monitor/login.cshtml");
        }
    }

    public IActionResult LogInfo()
    {
        Console.WriteLine("Log info");
        return View("/views/home/monitor/loginfo.cshtml");
    }

    public IActionResult RemoveSubAccount(string id)
    {
        Console.WriteLine("Remove sub account : " + id);
        AccountModel.RemoveSubAccount(id);
        return Redirect("/main/manageaccount");
    }

    public IActionResult RegistSubAccount(string id, string password)
    {
        Console.WriteLine("Regist sub account : " + id + " " + password);

        AccountModel.SaveSubAccount(id, password);


        return Redirect("/main/manageaccount");
    }
}

public class AccountHub: Hub
{
    public async Task LoginResult(string password)
    {
        int result;
        Console.WriteLine("Login result : " + password);

        result = AccountModel.CheckAccount("master", password);

        await Clients.All.SendAsync("LoginError", result);
    }
}