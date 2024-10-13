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
        public IActionResult InsertAdmin(Admin newadmin)
        {
            _context.Admins.Add(newadmin);
            _context.SaveChanges();
            return Ok();
        }
    }
}
