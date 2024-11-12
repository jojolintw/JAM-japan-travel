using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.Models;
using JP_FrontWebAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private JapanTravelContext _context;
        JWTService _jwtService;
        public CouponController(JapanTravelContext context, JWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        //取出所有我可以使用的優惠券
        [HttpGet("GetAllMycoupon")]
        [Authorize]
        public IActionResult GetAllMycoupon()
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);
                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyCouponDTO> allmyCoupons = _context.MemberCouponLists.Where(f => f.MemberId == mem.MemberId && f.Used==false).Select(s => new MyCouponDTO
                {
                    CouponId = s.CouponId,
                    CouponName = s.Coupon.CouponName,
                    Discount = s.Coupon.Discount,
                    ExpirationDate = s.Coupon.ExpirationDate.ToString("yyyy-MM-dd"),
                    MemberLevelId = Convert.ToInt32(s.Coupon.MemberLevelId)
                }).ToList();


                return Ok(allmyCoupons);
            }
            return Unauthorized(new { result = "noLogin" });
        }
        //取出所有我已使用的優惠券
        [HttpGet("GetAllMyUsedcoupon")]
        [Authorize]
        public IActionResult GetAllMyUsedcoupon()
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);
                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyCouponDTO> allmyusedCoupons = _context.MemberCouponLists.Where(f => f.MemberId == mem.MemberId && f.Used == true).Select(s => new MyCouponDTO
                {
                    CouponId = s.CouponId,
                    CouponName = s.Coupon.CouponName,
                    Discount = s.Coupon.Discount,
                    ExpirationDate = s.Coupon.ExpirationDate.ToString("yyyy-MM-dd"),
                    MemberLevelId = Convert.ToInt32(s.Coupon.MemberLevelId)
                }).ToList();


                return Ok(allmyusedCoupons);
            }
            return Unauthorized(new { result = "noLogin" });
        }
        //取出所有我可以領取的優惠券
        [HttpGet("GetAllMyAvailablecoupon")]
        //[Authorize]
        public IActionResult GetAllMyAvailablecoupon()
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);
                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                //找出會員所有的優惠券
                List<MyCouponDTO> allmyCoupons = _context.MemberCouponLists.Where(f => f.MemberId == mem.MemberId).Select(s => new MyCouponDTO
                {
                    CouponId = s.CouponId,
                    CouponName = s.Coupon.CouponName,
                    Discount = s.Coupon.Discount,
                    ExpirationDate = s.Coupon.ExpirationDate.ToString("yyyy-MM-dd"),
                    MemberLevelId = Convert.ToInt32(s.Coupon.MemberLevelId)
                }).ToList();


                //找出所有的優惠券
                List<MyCouponDTO> allCoupons = _context.Coupons.Select(s => new MyCouponDTO
                {
                    CouponId = s.CouponId,
                    CouponName = s.CouponName,
                    Discount = s.Discount,
                    ExpirationDate = s.ExpirationDate.ToString("yyyy-MM-dd"),
                    MemberLevelId = Convert.ToInt32(s.MemberLevelId)
                }).ToList();


            List<MyCouponDTO> AvailCoupons = allCoupons.Except(allmyCoupons, new MyCouponDTOComparer()).Where(s=>s.MemberLevelId<= mem.MemberLevelId).ToList();

            return Ok(AvailCoupons);
        }
            return Unauthorized(new { result = "noLogin" });
        }

        //==================================================================================================================================================
        public class MyCouponDTOComparer : IEqualityComparer<MyCouponDTO>
        {
            // 比較兩個 MyCouponDTO 物件的 CouponId 是否相等
            public bool Equals(MyCouponDTO x, MyCouponDTO y)
            {
                return x.CouponId == y.CouponId;
            }

            // 用 CouponId 生成哈希碼
            public int GetHashCode(MyCouponDTO obj)
            {
                return obj.CouponId.GetHashCode();
            }
        }

        //取出所有我可以使用的優惠券
        [HttpGet("Getoupon/{id}")]
        [Authorize]
        public IActionResult Getoupon(int id)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);
                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                MemberCouponList newmc = new MemberCouponList
                {
                    MemberId = mem.MemberId,
                    CouponId = id,
                    Used = false,
                };
                _context.Add(newmc);
                _context.SaveChanges();


                return Ok(new { result="success"});
            }
            return Unauthorized(new { result = "noLogin" });
        }


    }
}
