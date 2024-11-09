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
using JP_FrontWebAPI.Service;
using JP_FrontWebAPI.DTOs.Order;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private JapanTravelContext _context;
        private readonly EmailService _emailService;
        private JWTService _jwtService;
        public OrderController(JapanTravelContext context, EmailService emailService, JWTService jwtService)
        {
            _context = context;
            _emailService = emailService;
            _jwtService = jwtService;
        }
        
        [HttpGet("sendOrderInfoEmail")]
        public IActionResult sendOrderInfoEmail()
        {
            string to = "qwe58912@gmail.com";
            string subject = "test";
            string body = "hi";

            _emailService.SendEmailAsync(to, subject, body);

            return Ok((new {result="success"}));
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderData orderData)
        {

            if (orderData == null)
            {
                return BadRequest("Invalid data.");
            }

            //==============================

            Order order = new Order()
            {
                OrderNumber = "1" + DateTime.Now.ToString("yyMMddHHmmss"),
                MemberId = 1,
                OrderTime = DateTime.Now,
                PaymentMethodId = 2,
                OrderStatusId = 3,
                CouponId = orderData.couponId,
                TotalAmount = orderData.totalAmount,
                Remarks = orderData.remarks,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            
            foreach(var item in orderData.cart)
            {
                ItineraryOrderItem items = new ItineraryOrderItem()
                {
                    OrderId = order.OrderId,
                    ItineraryDateSystemId = item.itineraryDateSystemId,
                    Quantity = item.quantity,
                };

                _context.ItineraryOrderItems.Add(items);
                _context.SaveChanges();
            };

            //===============================

            return Ok(new
            {
                message = "資料接收成功！",
                receivedData = orderData
            });
        }

    }
}
