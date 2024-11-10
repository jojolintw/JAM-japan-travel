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
        public async Task<IActionResult> sendOrderInfoEmail([FromBody] OrderData orderData)
        {
            string orderContent = System.IO.File.ReadAllText("html/orderContent.html");

            orderContent = orderContent.Replace("{{userName}}", "Luke");

            string to = "qwe58912@gmail.com";
            string subject = "Japan Activity Memory (JAM) 訂單通知";
            //string body = GenerateHtmlEmail("Luke",orderData.cart,(decimal)orderData.totalAmount);
            string body = "123";

            await _emailService.SendEmailAsync(to, subject, body);

            return Ok((new {result="success"}));
        }


        //public string GenerateHtmlEmail(string customerName, List<CartItems> CartItems, decimal totalAmount)
        //{
        //    if (CartItems == null || CartItems.Count == 0)
        //    {
        //        throw new ArgumentException("購物車沒有商品資料", nameof(CartItems));
        //    }

        //    var htmlContent = @"
        //    <html>
        //    <body>
        //        <h1>訂單確認信</h1>
        //        <p>親愛的 " + customerName + @",</p>
        //        <p>感謝您在我們網站上購物，以下是您的訂單詳細資料：</p>
        //        <table border='1'>
        //            <tr><th>商品名稱</th><th>數量</th><th>單價</th></tr>";

        //    foreach (var item in CartItems)
        //    {
        //        htmlContent += $@"
        //        <tr>
        //            <td>{item.name}</td>
        //            <td>{item.quantity}</td>
        //            <td>NT$ {item.price}</td>
        //        </tr>";
        //    }

        //    htmlContent += $@"
        //        </table>
        //        <p><strong>總金額：NT$ {totalAmount}</strong></p>
        //        <p>若有任何問題，請隨時與我們聯繫。</p>
        //    </body>
        //    </html>";

        //    return htmlContent;
        //}

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
