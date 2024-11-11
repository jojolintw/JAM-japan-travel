using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using JP_FrontWebAPI.Service;
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
        JWTService _jwtService;
        public readonly IWebHostEnvironment _environ;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public MemberController(JapanTravelContext context, JWTService jwtService, IWebHostEnvironment environ, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtService = jwtService;
            _environ = environ;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("GetLoginMember")]
        [Authorize]
        public IActionResult GetLoginMember()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";


            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);
                var login = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);
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
                    loginDTO.ImageUrl = $"{baseUrl}/images/Member/{login.ImagePath}";
                var totoamount = _context.MemberTotalAmounts?.FirstOrDefault(m => m.MemberId == login.MemberId)?.TotalAmount;
                if(totoamount!= null)
                     loginDTO.TotalAmount = totoamount;
                return Ok(new { result = "success", loginmember = loginDTO });
            }

            return Unauthorized(new { result = "noLogin" });
        }

        // 修改會員資料==================================================================================================================
        [HttpPost("AlterMemberinformation")]
        [Authorize]
        public IActionResult AlterMemberinformation([FromForm] AlterMemberDTO memberDTO)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                var member = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

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
                member.Email = memberDTO.Email;

                //照片處理=============================================================
                if (memberDTO.file != null)
                {
                    string photoname = Guid.NewGuid() + ".jpg";
                    memberDTO.file.CopyTo(new FileStream(_environ.WebRootPath + "/images/Member/" + photoname, FileMode.Create));
                    member.ImagePath = photoname;
                }

                _context.SaveChanges();

                return Ok(new { result = "success"});

            }
            return Unauthorized(new { result = "noLogin" });
        }








        //===============================================================================================================================
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
