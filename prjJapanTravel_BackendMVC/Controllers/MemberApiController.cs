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
                Phone = inputmember.手機號碼,
                Email = inputmember.Email,
                Password = inputmember.密碼,
                MemberLevelId = inputmember.會員等級編號,
                MemberStatusId = inputmember.會員狀態編號
            };

            if (inputmember.photo != null) 
            {
                string photoname = Guid.NewGuid() + ".jpg";
                inputmember.photo.CopyTo(new FileStream(_environ.WebRootPath + "/images/Member/" + photoname, FileMode.Create));
                newmem.ImagePath = photoname;
            }
            _context.Members.Add(newmem);
            _context.SaveChanges();
//======================取得所有會員資料===================================================================
            var memberdatas = _context.Members.Select(m => new MemberViewModel
            {
                會員編號 = m.MemberId,
                會員姓名 = m.MemberName,
                性別 = (bool)m.Gender,
                生日 = Convert.ToDateTime(m.Birthday),
                城市 = m.City.City1,
                手機號碼 = m.Phone,
                Email = m.Email,
                密碼 = m.Password,
                會員等級 = m.MemberLevel.MemberLevelName,
                會員狀態 = m.MemberStatus.MemberStatusName,
                頭像路徑 = m.ImagePath
            });


            return Ok(memberdatas);
        }
        [HttpPost]
        public IActionResult AlterMember(MemberViewModel inputmember)
        {
            Member mem = _context.Members.FirstOrDefault(m => m.MemberId == inputmember.會員編號);
            mem.MemberName = inputmember.會員姓名;
            mem.Gender = inputmember.性別;
            mem.Birthday = inputmember.生日;
            mem.CityId = inputmember.城市編號;
            mem.Phone = inputmember.手機號碼;
            mem.Email = inputmember.Email;
            mem.Password = inputmember.密碼;
            mem.MemberLevelId = inputmember.會員等級編號;
            mem.MemberStatusId = inputmember.會員狀態編號;

            if (inputmember.photo != null)
            {
                string photoname = Guid.NewGuid() + ".jpg";
                inputmember.photo.CopyTo(new FileStream(_environ.WebRootPath + "/images/Member/" + photoname, FileMode.Create));
                mem.ImagePath = photoname;
            }
            _context.SaveChanges();
            //======================取得所有會員資料===================================================================
            var memberdatas = _context.Members.Select(m => new MemberViewModel
            {
                會員編號 = m.MemberId,
                會員姓名 = m.MemberName,
                性別 = (bool)m.Gender,
                生日 = Convert.ToDateTime(m.Birthday),
                城市 = m.City.City1,
                手機號碼 = m.Phone,
                Email = m.Email,
                密碼 = m.Password,
                會員等級 = m.MemberLevel.MemberLevelName,
                會員狀態 = m.MemberStatus.MemberStatusName,
                頭像路徑 = m.ImagePath
            });


            return Ok(memberdatas);
        }
        [HttpGet]
        public IActionResult DeleteMember(int id)
        {
            Member mem = _context.Members.FirstOrDefault(a => a.MemberId == id);
            _context.Members.Remove(mem);
            _context.SaveChanges();
            //======================取得所有會員資料===================================================================
            var memberdatas = _context.Members.Select(m => new MemberViewModel
            {
                會員編號 = m.MemberId,
                會員姓名 = m.MemberName,
                性別 = (bool)m.Gender,
                生日 = Convert.ToDateTime(m.Birthday),
                城市 = m.City.City1,
                手機號碼 = m.Phone,
                Email = m.Email,
                密碼 = m.Password,
                會員等級 = m.MemberLevel.MemberLevelName,
                會員狀態 = m.MemberStatus.MemberStatusName,
                頭像路徑 = m.ImagePath
            });

            return Json(memberdatas);
        }
        [HttpGet]
        public IActionResult Search(string Keyword)
        {
            if (Keyword == null)
            {
                var memberdatas = _context.Members.Select(m => new MemberViewModel
                {
                    會員編號 = m.MemberId,
                    會員姓名 = m.MemberName,
                    性別 = (bool)m.Gender,
                    生日 = Convert.ToDateTime(m.Birthday),
                    城市 = m.City.City1,
                    手機號碼 = m.Phone,
                    Email = m.Email,
                    密碼 = m.Password,
                    會員等級 = m.MemberLevel.MemberLevelName,
                    會員狀態 = m.MemberStatus.MemberStatusName,
                    頭像路徑 = m.ImagePath
                });
                return Json(memberdatas);
            }
            else
            {
                var allmembers = _context.Members.Where(a => a.MemberName.Contains(Keyword) || a.Phone.Contains(Keyword) || a.Email.Contains(Keyword)
                                                           || a.City.City1.Contains(Keyword) || a.MemberLevel.MemberLevelName.Contains(Keyword)
                                                           || a.MemberStatus.MemberStatusName.Contains(Keyword)).Select(m => new MemberViewModel
                                                           {
                                                               會員編號 = m.MemberId,
                                                               會員姓名 = m.MemberName,
                                                               性別 = (bool)m.Gender,
                                                               生日 = Convert.ToDateTime(m.Birthday),
                                                               城市 = m.City.City1,
                                                               手機號碼 = m.Phone,
                                                               Email = m.Email,
                                                               密碼 = m.Password,
                                                               會員等級 = m.MemberLevel.MemberLevelName,
                                                               會員狀態 = m.MemberStatus.MemberStatusName,
                                                               頭像路徑 = m.ImagePath
                                                           });
                return Json(allmembers);
            }

        }
    }
}
