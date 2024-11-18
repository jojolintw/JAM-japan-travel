using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using JP_FrontWebAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                if (login.EnglishName != null)
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
                if (totoamount != null)
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

                if (memberDTO != null)
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

                return Ok(new { result = "success" });

            }
            return Unauthorized(new { result = "noLogin" });
        }
        //========加入我的最愛========================================================================================================================
        [HttpGet("AddtoMyfavirite/{id}")]
        [Authorize]
        public IActionResult AddtoMyfavirite(int id)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                Itinerary iti = _context.Itineraries.FirstOrDefault(i => i.ItinerarySystemId == id);

                MyCollection mc = new MyCollection
                {
                    MemberId = mem.MemberId,
                    ItinerarySystemId = iti.ItinerarySystemId
                };

                _context.MyCollections.Add(mc);
                _context.SaveChanges();

                return Ok(new { result = "success" });
            }
            return Unauthorized(new { result = "noLogin" });

        }
        //移除我的最愛
        [HttpGet("RemoveMyfavirite/{id}")]
        [Authorize]
        public IActionResult RemoveMyfavirite(int id)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                Itinerary iti = _context.Itineraries.FirstOrDefault(i => i.ItinerarySystemId == id);

                MyCollection myCollection = _context.MyCollections.FirstOrDefault(f => f.MemberId == mem.MemberId && f.ItinerarySystemId == iti.ItinerarySystemId);

                _context.MyCollections.Remove(myCollection);

                _context.SaveChanges();

                return Ok(new { result = "success" });
            }
            return Unauthorized(new { result = "noLogin" });

        }
        //驗證是否為我的最愛
        [HttpGet("IsMyfavirite/{id}")]
        [Authorize]
        public IActionResult IsMyfavirite(int id)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                Itinerary iti = _context.Itineraries.FirstOrDefault(i => i.ItinerarySystemId == id);

                MyCollection myCollection = _context.MyCollections.FirstOrDefault(f => f.MemberId == mem.MemberId && f.ItinerarySystemId == iti.ItinerarySystemId);

                if (myCollection != null)
                {
                    return Ok(new { result = "ismyfavirite" });
                }

                return Ok(new { result = "isnotmyfavirite" });
            }
            return Unauthorized(new { result = "noLogin" });
        }
        //取出所有我的最愛
        [HttpGet("GetAllMyfavorite")]
        [Authorize]
        public IActionResult GetAllMyfavorite()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyFavoriteItilityDTO> allmyfavoriteItinerarys = _context.MyCollections.Where(f => f.MemberId == mem.MemberId).Select(s => new MyFavoriteItilityDTO
                {
                    ItinerarySystemId = s.ItinerarySystemId,
                    ItineraryName = s.ItinerarySystem.ItineraryName,
                    AreaSystemId = s.ItinerarySystem.AreaSystemId,
                    Price = s.ItinerarySystem.Price,
                    DepartureDate = s.ItinerarySystem.ItineraryDates.Where(p => p.ItinerarySystemId == s.ItinerarySystemId).Select(a => a.DepartureDate).ToList(),
                    ItineraryDetail = s.ItinerarySystem.ItineraryDetail,
                    Image = $"{baseUrl}/images/Product/{s.ItinerarySystem.Images.Where(p => p.ItinerarySystemId == s.ItinerarySystemId && p.ImageName == "封面").Select(a => a.ImagePath).FirstOrDefault()}"
                }).ToList();

                return Ok(allmyfavoriteItinerarys);
            }
            return Unauthorized(new { result = "noLogin" });
        }

        //取出地區分類的我的最愛
        [HttpGet("GetAreaMyfavorite/{id}")]
        [Authorize]
        public IActionResult GetAreaMyfavorite(int id)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyFavoriteItilityDTO> allmyfavoriteItinerarys = _context.MyCollections.Where(f => f.MemberId == mem.MemberId && f.ItinerarySystem.AreaSystemId == id).Select(s => new MyFavoriteItilityDTO
                {
                    ItinerarySystemId = s.ItinerarySystemId,
                    ItineraryName = s.ItinerarySystem.ItineraryName,
                    AreaSystemId = s.ItinerarySystem.AreaSystemId,
                    Price = s.ItinerarySystem.Price,
                    DepartureDate = s.ItinerarySystem.ItineraryDates.Select(s => s.DepartureDate).ToList(),
                    ItineraryDetail = s.ItinerarySystem.ItineraryDetail,
                    Image = $"{baseUrl}/images/Product/{s.ItinerarySystem.Images.Where(p => p.ItinerarySystemId == s.ItinerarySystemId && p.ImageName == "封面").Select(a => a.ImagePath).FirstOrDefault()}"
                }).ToList();

                return Ok(allmyfavoriteItinerarys);
            }
            return Unauthorized(new { result = "noLogin" });
        }
        //評論-------------------------------------------------------------------------------------------------------------------------------------------
        //取出評論的行程及OrderID
        [HttpGet("GetCommentOrderDetailId/{id}")]

        public IActionResult GetCommentOrderDetailId(int id)
        {

            ItineraryOrderItem io = _context.ItineraryOrderItems.FirstOrDefault(o => o.ItineraryOrderItemId == id);

            CommentDTO comment = new CommentDTO();

            comment.ordertetailId = io.ItineraryOrderItemId;
            comment.itinerarySystemId = io.ItineraryDateSystem.ItinerarySystem.ItinerarySystemId;
            comment.itineraryName = io.ItineraryDateSystem.ItinerarySystem.ItineraryName;
            if (io.CommentStar != null)
                comment.CommentStar = io.CommentStar;
            if (io.CommentContent != null)
                comment.CommentContent = io.CommentContent;
            if (io.CommentTime != null)
                comment.CommentTime = Convert.ToDateTime(Convert.ToDateTime(io.CommentTime).ToString("yyyy-MM-dd"));

            return Ok(comment);
        }

        [HttpPost("AlterComment")]
        public IActionResult AlterComment([FromBody] CommentDTO comment)
        {
            ItineraryOrderItem io = _context.ItineraryOrderItems.FirstOrDefault(o => o.ItineraryOrderItemId == comment.ordertetailId);
            io.CommentStar = comment.CommentStar;
            if (comment.CommentContent != null)
                io.CommentContent = comment.CommentContent;
            io.CommentTime = Convert.ToDateTime(Convert.ToDateTime(comment.CommentTime).ToString("yyyy-MM-dd"));
            _context.SaveChanges();
            return Ok(new { result = "success" });
        }
        //取得同一行程的所有評論===============================================================================================================================
        [HttpGet("GetAllCommentByItinerary/{id}")]
        public IActionResult GetAllCommentByItinerary(int id)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            List<AllCommentsByItinerary> allCommentsByItinerary = _context.ItineraryOrderItems.Where(i => i.ItineraryDateSystem.ItinerarySystem.ItinerarySystemId == id && i.CommentStar > 0).Select(s => new AllCommentsByItinerary
            {
                MemberID = Convert.ToInt32(s.Order.MemberId),
                MemberName = s.Order.Member.MemberName,
                MemberPhotoURL = $"{baseUrl}/images/Member/{s.Order.Member.ImagePath}",
                CommentStar = Convert.ToInt32(s.CommentStar),
                CommentContent = !string.IsNullOrEmpty(s.CommentContent) ? s.CommentContent : null,
                CommentTime = Convert.ToDateTime(s.CommentTime).ToString("yyyy-MM-dd")
            }).ToList();

                return Ok(allCommentsByItinerary);

        
        }
        //===============================================================================================================================
        [HttpGet("GetCityArea")]
        //[Authorize]
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
        [HttpGet("GetAllArea")]
        //[Authorize]
        public IActionResult GetAllArea()
        {
            var Areas = _context.Areas.Select(s => new AreaDTO
            {
                AreaSystemId = s.AreaSystemId,
                AreaName = s.AreaName,
            });
            return Ok(Areas);
        }
    }
}
