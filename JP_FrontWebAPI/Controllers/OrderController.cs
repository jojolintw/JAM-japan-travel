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
        private readonly LinePayService _linePayService;
        public OrderController(JapanTravelContext context, EmailService emailService, JWTService jwtService)
        {
            _context = context;
            _emailService = emailService;
            _jwtService = jwtService;
            _linePayService = new LinePayService();
        }
        
        //[HttpGet("sendOrderInfoEmail")]
        //public async Task<IActionResult> sendOrderInfoEmail()
        //{
        //    string to = "qwe58912@gmail.com";
        //    string subject = "Japan Activity Memory (JAM) 訂單通知";
        //    string body = "訂單通知";

        //    await _emailService.SendEmailAsync(to, subject, body);

        //    return Ok((new { result = "success" }));
        //}



        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderData orderData)
        {

            if (orderData == null)
            {
                return BadRequest("Invalid data.");
            }

            //==============================

           


            Order order = new Order()
            {
                OrderNumber = orderData.memberId + DateTime.Now.ToString("yyMMddHHmmss"),
                MemberId = orderData.memberId,
                OrderTime = DateTime.Now,
                PaymentMethodId = 2, //付款方式:LinePay
                OrderStatusId = 1,   //訂單狀態:已成立
                CouponId = 1,        //優惠券:驚喜大禮包-100
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



            var MemberName = _context.Members
               .Where(m => m.MemberId == orderData.memberId)
               .FirstOrDefault();

            string memberName = "";

            if (MemberName != null)
            {
                memberName = MemberName.MemberName;
            }

            // ============================== 發送訂單確認郵件 ==============================
            try
            {
                // 設定郵件內容
                string to = "luchienyu0313@gmail.com";  // 假設從 orderData 中獲取用戶的 email
                string subject = "Japan Activity Memory (JAM) 訂單通知";
                string body = $@"
                <!DOCTYPE html>
                <html lang='zh-tw'>
                <head>
                    <meta charset='utf-8' />
                    <title>訂單確認</title>
                    <style>
                        body {{
                            font-family: 'Arial', sans-serif;
                            background-color: #f4f4f9;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 800px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                        }}
                        h2 {{
                            color: #333333;
                            text-align: center;
                            font-size: 24px;
                            margin-bottom: 20px;
                        }}
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin-top: 20px;
                        }}
                        table th, table td {{
                            padding: 12px 15px;
                            text-align: left;
                            border-bottom: 1px solid #e0e0e0;
                        }}
                        table th {{
                            background-color: #f7f7f7;
                            color: #333;
                        }}
                        table tr:nth-child(even) {{
                            background-color: #f9f9f9;
                        }}
                        .total, .couponName {{
                            font-weight: bold;
                            color: #d9534f;
                            margin-top: 20px;
                        }}
                        .footer {{
                            font-size: 12px;
                            color: #888888;
                            text-align: center;
                            margin-top: 30px;
                        }}
                        .footer em {{
                            font-style: normal;
                            color: #555555;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>親愛的 {memberName}，感謝您的訂單！</h2>
                        <p>以下是您的訂單明細：</p>

                        <table>
                            <thead>
                                <tr>
                                    <th>商品名稱</th>
                                    <th>數量</th>
                                    <th>單價</th>
                                    <th>總價</th>
                                </tr>
                            </thead>
                            <tbody>";

                                // 訂單項目明細
                                foreach (var item in orderData.cart)
                                {
                                    body += $@"
                                <tr>
                                    <td>{item.name}</td>
                                    <td>{item.quantity}</td>
                                    <td>{item.price}</td>
                                    <td>{item.quantity * item.price}</td>
                                </tr>";
                                }

                                body += $@"
                            </tbody>
                        </table>

                        <p class='remarks'>備註：{orderData.remarks}</p>
                        <p class='couponName'>折扣金額：-100</p>
                        <p class='total'>總金額：{orderData.totalAmount.ToString()}</p>
        
                        <p>如果您有任何問題，請隨時聯繫我們。</p>
                        <p>祝您購物愉快！</p>
        
                        <div class='footer'>
                            <em>這是自動生成的郵件，請勿直接回覆。</em>
                        </div>
                    </div>
                </body>
                </html>";

                    // 發送郵件
                    await _emailService.SendEmailAsync(to, subject, body);
                }
                catch (Exception ex)
                {
                    // 發送郵件失敗，處理錯誤
                    return StatusCode(500, new { message = "訂單建立成功，但郵件發送失敗", error = ex.Message });
                }


                //===============================

                return Ok(new
                {
                    message = "資料接收成功！",
                    receivedData = orderData
                });
            }

    }
}
