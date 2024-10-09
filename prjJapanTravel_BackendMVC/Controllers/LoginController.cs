using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.LoginViewModels;

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
           Admin loginmember = _context.Admins.FirstOrDefault(a => a.Account.Equals(login.Account));
            if (loginmember == null) 
            {
                ErroMSg = "找不到帳號";
                return Json(new { result= "Noaccount" , message = ErroMSg });
            }

            if (!loginmember.Password.Equals(login.Password)) 
            {
                ErroMSg = "密碼輸入錯誤";
                ViewBag.ErrP = ErroMSg;
                return Json(new { result = "Nopassword", message = ErroMSg });
            }

            return Json(new { result = "OK" });
        }
    }
}
