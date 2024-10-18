using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        public IActionResult InsertMember(MemberViewModel inputmember)
        {
            Member newmem = new Member()
            {
                MemberName = inputmember.會員姓名,
                Gender = inputmember.性別,
                Birthday = inputmember.生日,
                CityId = inputmember.城市編號,
                Email = inputmember.Email,
                Password = inputmember.密碼,
                MemberLevelId = inputmember.會員等級編號,
                MemberStatusId = inputmember.會員狀態編號
            };

            if (inputmember.photo != null) 
            {
                string photoname = Guid.NewGuid() + ".jpg";
                inputmember.photo.CopyTo(new FileStream(_environ.WebRootPath + "/images/Admin/" + photoname, FileMode.Create));
                newmem.ImagePath = photoname;
            }
            _context.Members.Add(newmem);
            _context.SaveChanges();


            return Ok();
        }
    }
}
