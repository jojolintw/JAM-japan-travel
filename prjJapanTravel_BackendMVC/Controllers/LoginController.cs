using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.LoginViewModels;
using System.Text.Json;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class LoginController : Controller
    {
        private JapanTravelContext _context;


        public LoginController(JapanTravelContext context) 
        {
            _context = context;
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPage(LoginViewModel login)
        {
           string ErroMSg="";
           Admin loginAdmin = _context.Admins.FirstOrDefault(a => a.Account.Equals(login.Account));
            if (loginAdmin == null) 
            {
                ErroMSg = "找不到帳號";

                return Json(new { result= "Noaccount" , message = ErroMSg });
            }

            if (!loginAdmin.Password.Equals(login.Password)) 
            {
                ErroMSg = "密碼輸入錯誤";
                ViewBag.ErrP = ErroMSg;

                return Json(new { result = "Nopassword", message = ErroMSg });
            }

            string loginjson = JsonSerializer.Serialize(loginAdmin);
            HttpContext.Session.SetString(CDictionary.SK_LoginAdmin, loginjson);


            return Json(new { result = "OK" });
        }
    }
}
