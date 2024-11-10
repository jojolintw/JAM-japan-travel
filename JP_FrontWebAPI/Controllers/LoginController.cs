using Google.Apis.Auth;
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
using Microsoft.Extensions.Logging;
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
        private readonly JWTService _jwtService;
        public LoginController(JapanTravelContext context, EmailService emailService, JWTService jwtService) 
        {
            _context = context;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        //驗證是否有登入
        [HttpGet("islogin")]
        [Authorize]
        public IActionResult islogin()
        {
            return Ok(new {result = "successlogin" });
        }
        //google登入及註冊
        [HttpPost("googlelogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginInput gl)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(gl.token);
                // 驗證成功後處理使用者資訊，例如建立 JWT Token 或登入邏輯
                var userId = payload.Subject;
                var name = payload.Name;
                var email = payload.Email;
              

                var allemails = _context.Members.Select(e => e.Email).ToList();

                if (!allemails.Contains(email)) 
                {
                    Member newmember = new Member()
                    {
                        MemberName = name,
                        Email = email,
                        MemberLevelId = 1,
                        MemberStatusId = 1,
                        GoogleLink = true
                    };
                    _context.Members.Add(newmember);
                    _context.SaveChanges();
                    //找尋剛剛加入的member
                    var member = _context.Members.FirstOrDefault(e => e.Email == email);

                    JwtSecurityToken token=_jwtService.ProduceJWTToken(member);
                    return Ok(new { result = "successregester", token = new JwtSecurityTokenHandler().WriteToken(token) });
                }

                var mem = _context.Members.FirstOrDefault(e => e.Email == email);
                JwtSecurityToken tok = _jwtService.ProduceJWTToken(mem);

                // 返回或處理登入成功的結果
                return Ok(new { result = "successlogin", token = new JwtSecurityTokenHandler().WriteToken(tok) });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = "Invalid Google token", Error = ex.Message });
            }
            return Ok();
        }
        //登入==============================================================================================================
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
                JwtSecurityToken token = _jwtService.ProduceJWTToken(logmem);

                return Ok(new { result = "success" ,token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }
        //註冊===============================================================================================================
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
                MemberStatusId = 1,
                GoogleLink = false
            };

            _context.Members.Add(newmember);
            _context.SaveChanges();

            //產生JWT Token

            JwtSecurityToken token = _jwtService.ProduceJWTToken(newmember);

            return Ok(new { result = "success", message = "註冊成功", token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        //寄認證信==============================================================================================================
        [HttpGet("sendCertificationEmail")]
        [Authorize]
        public IActionResult sendCertificationEmail()
        {
            string subject = "會員認證信";
            string body = "<a href=\"http://localhost:4200/login/certificationsuccess\">點擊完成認證</a>";

            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member member = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                _emailService.SendEmailAsync("chaosabyss73@gmail.com", subject, body);
                return Ok((new { result = "success" }));
            }
            return Ok((new { result = "fail" }));
        }
        //修改會員認證=============================================================================================================
        [HttpGet("memberCertification")]
        [Authorize]
        public IActionResult memberCertification()
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member member = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);
                member.MemberStatusId = 2;
                _context.SaveChanges();
                return Ok((new { result = "success" }));
            }
            return Ok((new { result = "fail" }));
        }
        //忘記密碼驗證信
        [HttpPost("forgetpasswordEmail")]
        public IActionResult forgetpasswordEmail([FromBody] LoginInput l)
        {
                //找會員
                Member forgetpasswordmember = _context.Members.FirstOrDefault(m => m.Email == l.Email);
            if (forgetpasswordmember == null) 
            {
                return Ok((new { result = "fail", message="查無此人" }));
            }

            string recipientEmail = "chaosabyss73@gmail.com";
            string subject = "重設密碼";
            string body = "<a href=\"http://localhost:4200/login/resetpassword\">點擊重設密碼</a>";

            _emailService.SendEmailAsync(recipientEmail, subject, body);

               return Ok((new { result = "success" }));         
        }
        [HttpPost("resetPassword")]
        public IActionResult resetPassword([FromBody] LoginInput l)
        {
            //找會員
            Member forgetpasswordmember = _context.Members.FirstOrDefault(m => m.Email == l.Email);
            if (forgetpasswordmember == null)
            {
                return Ok((new { result = "fail", message = "查無此人" }));
            }
            forgetpasswordmember.Password = l.Password;
            _context.SaveChanges();
            return Ok((new { result = "success" }));
        }
    }  
}

