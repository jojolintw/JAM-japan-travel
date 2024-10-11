using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using prjJapanTravel_BackendMVC.Models;
using System.Text.Json;

namespace prjJapanTravel_BackendMVC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
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
    }
}
