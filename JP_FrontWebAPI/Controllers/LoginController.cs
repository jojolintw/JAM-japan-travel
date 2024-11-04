using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using JP_FrontWebAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JapanTravelContext _context;
        private readonly EmailService _emailService;
        public LoginController(JapanTravelContext context, EmailService emailService) 
        {
            _context = context;
            _emailService = emailService;
        }
        //登入
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInput l)
        {
            string ErroMSg = "";
            Member logmem = _context.Members.FirstOrDefault(m => m.Email == l.Email);
            if (logmem == null)
            {
                ErroMSg = "找不到帳號";

                return Ok(new { result = "ErrorAccount", message = ErroMSg });
            }
            if (!l.Password.Equals(logmem.Password))
            {
                ErroMSg = "密碼錯誤";
                

                return Ok(new { result = "ErrorPassword", message = ErroMSg });
            }

            // 驗證用戶名和密碼
            if (l.Email == logmem.Email && l.Password == logmem.Password) // 這裡用真實驗證邏輯
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, l.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Setting.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7100",
                    audience: "http://localhost:4200",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                return Ok(new { result = "success" ,token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }
        //註冊
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDTO regDTO) 
        {
            string ErrorMessage = "";
            var AllMemberEmails = _context.Members.Select(m => m.Email);
            if (AllMemberEmails.Contains(regDTO.RegisterEmail)) 
            {
                ErrorMessage = "有重複的帳號";
                return Ok(new { result = "repeataccount", message= ErrorMessage });
            }

            Member newmember = new Member()
            {
                MemberName = regDTO.RegisterName,
                Email = regDTO.RegisterEmail,
                Password = regDTO.RegisterPassword,
                MemberLevelId = 1,
                MemberStatusId = 1
            };

            _context.Members.Add(newmember);
            _context.SaveChanges();

            //產生JWT Token
            var claims = new[]
               {
                new Claim(JwtRegisteredClaimNames.Sub, regDTO.RegisterEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Setting.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7100",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new { result = "success", message = "註冊成功", token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        //寄認證信
        [HttpGet("sendCertificationEmail")]
        public IActionResult sendCertificationEmail()
        {
            string subject = "會員認證信";
            string body = "<a href=\"http://localhost:4200/login/certificationsuccess\">點擊完成認證</a>";

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
                _emailService.SendEmailAsync("chaosabyss73@gmail.com", subject, body);
                return Ok((new { result = "success" }));
            }
            return Ok((new { result = "fail" }));
        }
        //會員認證
        [HttpGet("memberCertification")]
        public IActionResult memberCertification()
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

                //找會員
                Member certificationmem = _context.Members.FirstOrDefault(m => m.Email == useremail);
                certificationmem.MemberStatusId = 2;
                _context.SaveChanges();
                return Ok((new { result = "success" }));
            }
            return Ok((new { result = "fail" }));
        }
    }  
}

