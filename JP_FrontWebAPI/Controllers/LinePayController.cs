using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Sockets;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using JP_FrontWebAPI.DTOs.Order;
using JP_FrontWebAPI.Service;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Text;
using JP_FrontWebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using static JP_FrontWebAPI.DTOs.Order.LinepayDTO;




namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;
        public LinePayController(LinePayService linePayService)
        {
            _linePayService = linePayService;
        }

        [HttpPost("RequestAPI")]
        public async Task<PaymentResponseDto> RequestAPI(PaymentRequestDto dto)
        {
            return await _linePayService.Request(dto);
        }

        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmAPI([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        {
            return await _linePayService.Confirm(transactionId, orderId, dto);
        }












        //private readonly HttpClient _httpClient;
        //private readonly LinePayService _linePayService;
        
        //public LinePayController(HttpClient httpClient)
        //{
        //    _linePayService = new LinePayService();
        //    _httpClient = httpClient;
        //}


        //[HttpPost("LinePay")]
        //public async Task<IActionResult> LinePay([FromBody] LineOrderData lineOrderData)
        //{
        //    // 獲取ngrok URL
        //    string ngrokUrl = await GetNgrokPublicUrl();


        //    // 構建Line Pay API 請求的訂單資料
        //    var orderData = new
        //    {
        //        amount = lineOrderData.amount,
        //        currency = "TWD",
        //        orderId = "JAM" + lineOrderData.orderId,
        //        confirmUrl = $"{ngrokUrl}/orderconfirmation", // 回調URL
        //        cancelUrl = $"{ngrokUrl}/itinerary-list",   // 取消支付的URL
        //    };


        //    // 向 Line Pay API 發送訂單創建請求
        //    string linePayApiUrl = "https://sandbox-api-pay.line.me/v3/payments/request";

        //    string jsonContent = JsonConvert.SerializeObject(orderData);
        //    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        //    // 生成 X-LINE-Authorization-Nonce (防止重放攻擊)
        //    string nonce = Guid.NewGuid().ToString();

        //    // 計算 X-LINE-Authorization（基於 ChannelSecret 和請求的內容）
        //    string signature = GenerateAuthorizationSignature(orderData, nonce);


        //    _httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", "2006530351");
        //    _httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", "96f3c66527b68f45c7dee92962c58855");
        //    _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
        //    _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);


        //    HttpResponseMessage response = await _httpClient.PostAsync(linePayApiUrl, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        var responseData = JsonConvert.DeserializeObject<dynamic>(result);
        //        string paymentUrl = responseData.info.paymentUrl.web;   //獲取LinePay支付頁面URL

        //        // 返回給前端支付頁面URL
        //        return Ok(new { paymentUrl });
        //    }
        //    else
        //    {
        //        var errorResponse = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"LinePay API error: {errorResponse}");
        //        return StatusCode(500, $"Line Pay API error: {errorResponse}");
        //    }
        //}

        //// 用於生成 X-LINE-Authorization 签名的函数
        //private string GenerateAuthorizationSignature(object orderData, string nonce)
        //{
        //    string channelSecret = "96f3c66527b68f45c7dee92962c58855";  // 你的 ChannelSecret
        //    string payload = "POST /v3/payments/request" + JsonConvert.SerializeObject(orderData) + nonce;

        //    // 使用 HMACSHA256 計算簽名
        //    using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(channelSecret)))
        //    {
        //        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
        //        return BitConverter.ToString(hash).Replace("-", "").ToLower();  // 返回簽名
        //    }
        //}



        //// 獲取ngrok的公開URL
        //private async Task<string> GetNgrokPublicUrl()
        //{
        //    string ngrokApiUrl = "http://localhost:4040/api/tunnels";
        //    HttpResponseMessage response = await _httpClient.GetAsync(ngrokApiUrl);
        //    string jsonResponse = await response.Content.ReadAsStringAsync();
        //    dynamic result = JsonConvert.DeserializeObject(jsonResponse);

        //    // 返回ngrok的公開URL
        //    return result.tunnels[0].public_url;
        //}
    }
}
