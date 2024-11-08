using System.Net.Mail;
using System.Net;
using JP_FrontWebAPI.DTOs.Member;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JP_FrontWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JP_FrontWebAPI.Service
{
    public class JWTService
    {
        //產生JWT Token
        public JwtSecurityToken ProduceJWTToken(Member logmem)
        {
            var claims = new[]
               {
                new Claim(JwtRegisteredClaimNames.Sub, logmem.MemberId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Setting.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:7100",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            return token;
        }

        public int CertificationJWTToken(string authorizationHeader)
        {
                // 取出 JWT Token
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // 如果需要進一步解析 JWT Token，可使用 JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 取得 Token 的相關資訊 (如使用者名稱等)

                //var member = JsonSerializer.Deserialize<>(jwtToken);
                int  memberId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value);

                return memberId;
        }
    }
}
