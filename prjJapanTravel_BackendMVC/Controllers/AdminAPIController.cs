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
        IWebHostEnvironment _environ;

        public AdminAPIController(JapanTravelContext context, IWebHostEnvironment environ) 
        {
            _context=context;
            _environ = environ;
        }


        [HttpGet]
        public IActionResult GetData(int adminId)
        {
            Admin ad = _context.Admins.FirstOrDefault(a => a.AdminId == adminId);
            return Ok(ad);
        }
        //AdminAPI/InsertAdmin
        [HttpPost]
        public IActionResult InsertAdmin(AdminViewModel inputAdmin)
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

            if (inputAdmin.photo != null) 
            { 
                inputAdmin.photo.CopyTo(new FileStream(_environ.WebRootPath + "/images/" + inputAdmin.photo.FileName, FileMode.Create));
                admin.ImagePath = inputAdmin.photo.FileName;
            }

            _context.Admins.Add(admin);
            _context.SaveChanges();
            var alladmins = _context.Admins.Select(a => a);
            return Json(alladmins);
        }
        [HttpPost]
        public IActionResult AlterAdmin(AdminViewModel inputAdmin)
        {
            Admin admin = _context.Admins.FirstOrDefault(a => a.AdminId == inputAdmin.AdminId);
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

            if (inputAdmin.photo != null)
            {
                inputAdmin.photo.CopyTo(new FileStream(_environ.WebRootPath + "/images/Admin/" + inputAdmin.photo.FileName, FileMode.Create));
                admin.ImagePath = inputAdmin.photo.FileName;
            }

            _context.SaveChanges();
            var alladmins = _context.Admins.Select(a => a);
            return Json(alladmins);
        }
    }
}
