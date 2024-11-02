using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private JapanTravelContext _context;
        public LoginController(JapanTravelContext context) 
        {
            _context = context;
        }

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
            var sessionId = HttpContext.Session.Id;
            //string loginjson = JsonSerializer.Serialize(logmem);
            //HttpContext.Session.SetString(CDictionary.SK_LoginMember, loginjson);
            //string memberjson = HttpContext.Session.GetString(CDictionary.SK_LoginMember);

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
                    audience: "https://localhost:4200",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                return Ok(new { result = "success" ,token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}

