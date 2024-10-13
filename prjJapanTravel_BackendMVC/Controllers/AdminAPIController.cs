using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.AdminViewModels;
using System.Text.Json;

namespace prjJapanTravel_BackendMVC.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class AdminAPIController : Controller
    {
        public JapanTravelContext _context;

        public AdminAPIController(JapanTravelContext context) 
        {
            _context=context;
        }


        [HttpGet]
        public IActionResult GetData(int adminId)
        {
            Admin ad = _context.Admins.FirstOrDefault(a => a.AdminId == adminId);
            return Ok(ad);
        }
        //AdminAPI/InsertAdmin
        [HttpPost]
        public IActionResult InsertAdmin(InsertAdminViewModel inputAdmin)
        {
            Admin admin = new Admin();
            admin.AdminName = inputAdmin.AdminName;
            admin.Account = inputAdmin.Account;
            admin.Password = inputAdmin.Password;
            admin.AdminManageStatus = inputAdmin.AdminManageStatus;
            admin.MemberManageStatus = inputAdmin.MemberManageStatus;
            admin.IniteraryManageStatus = inputAdmin.IniteraryManageStatus;
            admin.ShipmentManageStatus = inputAdmin.ShipmentManageStatus;
            admin.OrderManageStatus = inputAdmin.OrderManageStatus;
            admin.CouponManageStatus = inputAdmin.CouponManageStatus;
            admin.CommentManageStatus = inputAdmin.CommentManageStatus;
            admin.BlogManageStatus = inputAdmin.BlogManageStatus;
            
            _context.Admins.Add(admin);
            _context.SaveChanges();
            var alladmins = _context.Admins.Select(a => a);
            return Ok(alladmins);
        }
    }
}
