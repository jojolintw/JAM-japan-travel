using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Sockets;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using JP_FrontWebAPI.DTOs.Order;
using JP_FrontWebAPI.Service;
using Microsoft.EntityFrameworkCore;


namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {


        private readonly HttpClient _httpClient;
        private readonly LinePayService _linePayService;

        public PaymentController(HttpClient httpClient)
        {
            _linePayService = new LinePayService();
            _httpClient = httpClient;
        }


        [HttpPost]
        public async Task<IActionResult> LinePay([FromBody] LineOrderData lineOrderData)
        {
            // 獲取ngrok URL
            string ngrokUrl = await GetNgrokPublicUrl();


            // 構建Line Pay API 請求的訂單資料
            var orderData = new
            {
                amount = lineOrderData.amount,
                currency = lineOrderData.currency,
                orderId = lineOrderData.orderId,
                confirmUrl = $"{ngrokUrl}/payment/confirm", // 回調URL
                cancelUrl = $"{ngrokUrl}/payment/cancel",   // 取消支付的URL
            };


            // 向 Line Pay API 發送訂單創建請求
            string linePayApiUrl = "https://sandbox-api-pay.line.me/v3/payments/request";

            string jsonContent = JsonConvert.SerializeObject(orderData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");


            _httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", "2006530351");
            _httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", "96f3c66527b68f45c7dee92962c58855");


            HttpResponseMessage response = await _httpClient.PostAsync(linePayApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(result);
                string paymentUrl = responseData.info.paymentUrl.web;   //獲取LinePay支付頁面URL

                // 返回給前端支付頁面URL
                return Ok(new { paymentUrl });
            }
            else
            {
                return StatusCode(500, "Line Pay API error");
            }
        }

        // 獲取ngrok的公開URL
        private async Task<string> GetNgrokPublicUrl()
        {
            string ngrokApiUrl = "http://localhost:4040/api/tunnels";
            HttpResponseMessage response = await _httpClient.GetAsync(ngrokApiUrl);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);

            // 返回ngrok的公開URL
            return result.tunnels[0].public_url;
        }
    }
}
