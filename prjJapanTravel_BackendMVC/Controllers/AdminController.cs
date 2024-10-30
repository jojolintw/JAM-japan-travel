using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace prjJapanTravel_BackendMVC.Controllers
{
  
    public class AdminController : Controller
    {
        public JapanTravelContext _context;

        public AdminController(JapanTravelContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult AccessAdmin()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LoginAdmin))
             return Json(new { success = false, errormessage = "未登入" });
           


            string adminjson = HttpContext.Session.GetString(CDictionary.SK_LoginAdmin);
            Admin loginAdmin = JsonSerializer.Deserialize<Admin>(adminjson);

            if(loginAdmin.AdminManageStatus == false)
                return Json(new { success = false, errormessage = "無權限" });

            return Json(new { success = true});

        }
        public IActionResult AdminList()
        {
            var datas = _context.Admins;

            return PartialView("_AdminListPartial", datas);
        }

    }
}
