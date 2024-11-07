using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private JapanTravelContext _context;
        public readonly IWebHostEnvironment _environ;
        public MemberController(JapanTravelContext context, IWebHostEnvironment environ)
        {
            _context = context;
            _environ = environ;
        }
        [HttpGet("GetLoginMember")]
        [Authorize]
        public IActionResult GetLoginMember()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {        
                // 取出 JWT Token
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // 如果需要進一步解析 JWT Token，可使用 JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 取得 Token 的相關資訊 (如使用者名稱等)

                //var member = JsonSerializer.Deserialize<>(jwtToken);
                var useremail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                //從後端Session中取出登入者資料
                //string memberjson = HttpContext.Session.GetString(CDictionary.SK_LoginMember);
                //Member loginMember = JsonSerializer.Deserialize<Member>(memberjson);
                //驗證後端Session及JWT Token中的email是相同的
                //if (loginMember.Email == email) 
                //{
                var login = _context.Members.FirstOrDefault(m => m.Email == useremail);
                LoginMemberDTO loginDTO = new LoginMemberDTO();
                loginDTO.MemberId = login.MemberId;
                loginDTO.ChineseName = login.MemberName;
                if(login.EnglishName!= null)
                    loginDTO.EnglishName = login.EnglishName;
                if (login.Gender != null)
                    if (login.Gender == true)
                    {
                        loginDTO.Gender = "true";
                    }
                    else 
                    {
                        loginDTO.Gender = "false";
                    }
                    
                if (login.Birthday != null)
                    loginDTO.Birthday = login.Birthday;
                if (login.City?.CityAreaId != null)
                    loginDTO.CityAreaId = login.City.CityAreaId;
                if (login.City?.CityName != null)
                    loginDTO.CityAreaName = login.City.CityArea.CityAreaName;
                if (login.CityId != null)
                    loginDTO.CityId = login.CityId;
                if (login.City?.CityName != null)
                    loginDTO.CityName = login.City.CityName;
                if (login.Phone != null)
                    loginDTO.Phone = login.Phone;
                loginDTO.Email = login.Email;
                loginDTO.MemberLevelId = login.MemberLevelId;
                loginDTO.MemberLevel = login.MemberLevel.MemberLevelName;
                loginDTO.MemberStatusId = login.MemberStatusId;
                loginDTO.MemberStatus = login.MemberStatus.MemberStatusName;
                if (login.ImagePath != null)
                    loginDTO.Photopath = login.ImagePath;

                    return Ok(new { result = "success", loginmember = loginDTO });
            }

            return Unauthorized(new { result = "noLogin" });
        }
        [HttpPost("AlterMemberinformation")]
        [Authorize]
        public IActionResult AlterMemberinformation([FromForm] AlterMemberDTO memberDTO)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                // 取出 JWT Token
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // 如果需要進一步解析 JWT Token，可使用 JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 取得 Token 的相關資訊 (如使用者名稱等)

                var useremail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                // 更改會員
                 var member = _context.Members.FirstOrDefault(m => m.Email == useremail);

                if(memberDTO != null)
                    member.MemberName = memberDTO.MemberName;
                if (memberDTO.EnglishName != null)
                    member.EnglishName = memberDTO.EnglishName;
                if (memberDTO.Gender != null) 
                {
                    bool gender = true;
                    if (memberDTO.Gender == "false") 
                    {
                        gender = !gender;
                    }
                    member.Gender = gender;
                }
               
                if (memberDTO.Birthday != null)
                    member.Birthday = Convert.ToDateTime(memberDTO.Birthday);
                if (memberDTO.CityId != null)
                    member.CityId = memberDTO.CityId;
                if (memberDTO.Phone != null)
                    member.Phone = memberDTO.Phone;

                //照片處理=============================================================
                if (memberDTO.file != null)
                {
                    string photoname = Guid.NewGuid() + ".jpg";
                    memberDTO.file.CopyTo(new FileStream(_environ.WebRootPath + "/images/Member/" + photoname, FileMode.Create));
                    member.ImagePath = photoname;
                }

                _context.SaveChanges();

                return Ok(new { result = "success", memberinformation = useremail });

            }
            return Unauthorized(new { result = "noLogin" });
        }
        [HttpGet("GetCityArea")]
        [Authorize]
        public IActionResult GetCityArea() 
        {
            var CityAreas = _context.CityAreas.Select(s => new CityAreaDTO 
            {
                CityAreaId = s.CityAreaId,
                CityAreaName = s.CityAreaName,
            });
            return Ok(CityAreas);
        }
        [HttpGet("GetCity/{id}")]
        //[Authorize]
        public IActionResult GetCity(int id)
        {
            var Citys = _context.Cities.Where(c => c.CityAreaId == id).Select(s => new CityDTO
            {
                CityId = s.CityId,
                CityName = s.CityName,
            });
            return Ok(Citys);
        }
    }
}
