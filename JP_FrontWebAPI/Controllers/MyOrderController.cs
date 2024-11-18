using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.Models;
using JP_FrontWebAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JP_FrontWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyOrderController : ControllerBase
    {
        private readonly JapanTravelContext _context;
        private readonly JWTService _jwtService;
        public MyOrderController(JapanTravelContext context, JWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("GetAllMyOrders")]
        [Authorize]
        public IActionResult GetAllMyOrders()
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyOrderDTO> myorders = _context.Orders.Where(o => o.MemberId == mem.MemberId).Select(s => new MyOrderDTO
                {
                    OrderId = s.OrderId,
                    OrderNumber = s.OrderNumber,
                    MemberId = mem.MemberId,
                    MemberName = mem.MemberName,
                    OrderStatusId = Convert.ToInt32(s.OrderStatusId),
                    OrderStatus = s.OrderStatus.OrderStatus1,
                    PaymentMehtodId = Convert.ToInt32(s.PaymentMethodId),
                    PaymentMethod = s.PaymentMethod.PaymentMethod1,
                    TotalAmount = Convert.ToDecimal(s.TotalAmount).ToString("###,###"),
                    OrderTime = Convert.ToDateTime(s.OrderTime).ToString("yyyy-MM-dd") 
                }).ToList();


                return Ok(myorders);
            }
            return Unauthorized(new { result = "noLogin" });
         }

        [HttpGet("GetOrderDetail/{id}")]
        [Authorize]
        public IActionResult GetOrderDetail(int id)
        {
            //取出JWT
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {

                int loginMemberId = _jwtService.CertificationJWTToken(authorizationHeader);

                Member mem = _context.Members.FirstOrDefault(m => m.MemberId == loginMemberId);

                List<MyOrderDetailDTO> orderdetails = _context.ItineraryOrderItems.Where(od => od.OrderId == id).Select(s => new MyOrderDetailDTO
                {
                    OrderDetailId = s.ItineraryOrderItemId,
                    OrderId = Convert.ToInt32(s.OrderId),
                    OrderNumber= s.Order.OrderNumber,
                    ItineraryDateSystemId = Convert.ToInt32(s.ItineraryDateSystemId),
                    ItineraryId = s.ItineraryDateSystem.ItinerarySystem.ItineraryId,
                    ItinerarySystemId = Convert.ToInt32(s.ItineraryDateSystem.ItinerarySystemId),
                    ItineraryName = s.ItineraryDateSystem.ItinerarySystem.ItineraryName,
                    DepartureDate = Convert.ToDateTime(s.ItineraryDateSystem.DepartureDate).ToString("yyyy-MM-dd HH:mm"),
                    Quantity =Convert.ToInt32(s.Quantity),
                    TotalPrice = (Convert.ToInt32(s.ItineraryDateSystem.ItinerarySystem.Price)).ToString("###,###")
                }).ToList(); 


                return Ok(orderdetails);
        }
            return Unauthorized(new { result = "noLogin" });
        }
    }
}
