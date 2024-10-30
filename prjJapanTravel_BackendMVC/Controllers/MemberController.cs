using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.MemberViewModels;
using System.Text.Json;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class MemberController : Controller
    {
        public JapanTravelContext _context;

        public MemberController(JapanTravelContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult AccessMember()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LoginAdmin))
                return Json(new { success = false, errormessage = "未登入" });



            string adminjson = HttpContext.Session.GetString(CDictionary.SK_LoginAdmin);
            Admin loginAdmin = JsonSerializer.Deserialize<Admin>(adminjson);

            if (loginAdmin.MemberManageStatus == false)
                return Json(new { success = false, errormessage = "無權限" });

            return Json(new { success = true });

        }
        public IActionResult MemberList()
        {
            var datas = _context.Members.Select(m =>new MemberViewModel
            {
                會員編號 = m.MemberId,
                會員姓名 = m.MemberName,
                性別 =(bool)m.Gender,
                生日 = Convert.ToDateTime(m.Birthday),
                //城市 = m.City.City1,
                手機號碼 = m.Phone,
                Email = m.Email,
                密碼 = m.Password,
                會員等級 = m.MemberLevel.MemberLevelName,
                會員狀態 = m.MemberStatus.MemberStatusName,
                頭像路徑 = m.ImagePath
            });
            return PartialView("_MemberListPartial", datas);
        }
    }
}
