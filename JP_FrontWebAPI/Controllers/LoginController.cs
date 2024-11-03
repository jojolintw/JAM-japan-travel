using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            // 驗證用戶名和密碼
            if (l.Email == "winnie1945" && l.Password == "w12345") // 這裡用真實驗證邏輯
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

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
        [HttpGet("GetLoginMember")]
        [Authorize]
        public IActionResult GetLoginMember()
        {
            var members = _context.Members;
            //var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            //if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            //{
            //    // 取出 JWT Token
            //    var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            //    // 如果需要進一步解析 JWT Token，可使用 JwtSecurityTokenHandler
            //    var handler = new JwtSecurityTokenHandler();
            //    var jwtToken = handler.ReadJwtToken(token);

            //    // 取得 Token 的相關資訊 (如使用者名稱等)

            //    //var member = JsonSerializer.Deserialize<>(jwtToken);
            //    var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

            //    return Ok(new { Message = "Token 解析成功" });
            //}

            //return Unauthorized(new { Message = "無法取得 Token 或 Token 格式不正確" });
            return Ok(members);
        }
    }
}

