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
using System.Net;
using System.Net.Mail;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private JapanTravelContext _context;
        public OrderController(JapanTravelContext context)
        {
            _context = context;
        }
        [HttpGet("GetLoginMember")]
        [Authorize]
        public IActionResult GetLoginMember()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                // 取出 JWT Token
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // 如果需要進一步解析 JWT Token，可使用 JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 取得 Token 的相關資訊 (如使用者名稱等)

                //var member = JsonSerializer.Deserialize<>(jwtToken);
                var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                //從後端Session中取出登入者資料
                //string memberjson = HttpContext.Session.GetString(CDictionary.SK_LoginMember);
                //Member loginMember = JsonSerializer.Deserialize<Member>(memberjson);
                //驗證後端Session及JWT Token中的email是相同的
                //if (loginMember.Email == email) 
                //{
                var login = _context.Members.FirstOrDefault(m => m.Email == email);
                LoginMemberDTO loginDTO = new LoginMemberDTO();
                loginDTO.MemberId = login.MemberId;
                loginDTO.ChineseName = login.MemberName;
                if (login.EnglishName != null)
                    loginDTO.EnglishName = login.EnglishName;
                if (login.Gender != null)
                    loginDTO.Gender = login.Gender;
                if (login.Birthday != null)
                    loginDTO.Birthday = login.Birthday;
                if (login.City.CityAreaId != null)
                    loginDTO.CityAreaId = login.City.CityAreaId;
                if (login.City.CityName != null)
                    loginDTO.CityAreaName = login.City.CityArea.CityAreaName;
                if (login.CityId != null)
                    loginDTO.CityId = login.CityId;
                if (login.City.CityName != null)
                    loginDTO.CityName = login.City.CityName;
                if (login.Phone != null)
                    loginDTO.Phone = login.Phone;
                loginDTO.Email = login.Email;
                loginDTO.Password = login.Password;
                loginDTO.MemberLevelId = login.MemberLevelId;
                loginDTO.MemberLevel = login.MemberLevel.MemberLevelName;
                loginDTO.MemberStatusId = login.MemberStatusId;
                loginDTO.MemberStatus = login.MemberStatus.MemberStatusName;
                if (login.ImagePath != null)
                    loginDTO.Photopath = login.ImagePath;

                return Ok(new { result = "succcess", loginmember = loginDTO });
                //}
                //return Ok(new { result = "inconsistent" });
            }

            return Unauthorized(new { result = "noLogin" });
        }


        //public void sendEmail(string to, string subject, string body)
        //{
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("myemail@gmail.com", "password"),
        //        EnableSsl = true,
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("myemail@gmail.com"),
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true,
        //    };

        //    mailMessage.To.Add(to);

        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        Console.WriteLine("郵件發送成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"發送錯誤: {ex.Message}");
        //    }
        //}
    }
}
