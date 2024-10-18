using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.MemberViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class MemberApiController : Controller
    {
        public readonly JapanTravelContext _context;
        public readonly IWebHostEnvironment _environ;
        public MemberApiController(JapanTravelContext context, IWebHostEnvironment environ)
        {
            _context = context;
            _environ = environ;
        }

        [HttpGet]
        public IActionResult GetAreaList()
        {
            var cities = _context.Cities;
            return Ok(cities);
        }
        [HttpGet]
        public IActionResult GetLevelList()
        {
            var MemberLevels = _context.MemberLevels;
            return Ok(MemberLevels);
        }
        [HttpGet]
        public IActionResult GetMemberData(int memberId)
        {
            Member mem  = _context.Members.FirstOrDefault(m => m.MemberId == memberId);

            return Ok(mem);
        }
        [HttpPost]
        public IActionResult InsertMember(MemberViewModel m)
        {


            return Ok();
        }
    }
}
