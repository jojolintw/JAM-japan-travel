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
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using JP_FrontWebAPI.Common;




namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        //private readonly JwtService _jwtService;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string channelId = "2006530351";  // 從 LINE 開發者平台取得
        private readonly string channelSecretKey = "96f3c66527b68f45c7dee92962c58855";  // 從 LINE 開發者平台取得
        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";  // Sandbox API URL
        private static readonly Dictionary<string, CheckOutDTO> OrderPayments = new Dictionary<string, CheckOutDTO>();//儲存訂單編號對應的金額
        private static readonly Dictionary<string, string> OrderToken = new Dictionary<string, string>();
        //private readonly NotificationService _notificationService;
        //private readonly IHubContext<NotificationHub> _hubContext;

        public LinePayController(HttpClient httpClient, IMemoryCache memoryCache/*, NotificationService notificationService, IHubContext<NotificationHub> hubContext*/)
        {
            _httpClient = httpClient;
            //_jwtService = jwtService;
            _memoryCache = memoryCache;
            //_hubContext = hubContext;
            //_notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestPayment([FromBody] CheckOutDTO checkOutDto)
        {
            // 使用 JwtService 來獲取 fUserId
            //var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
            //if (result != null)
            //{
            //    // 如果有錯誤，根據具體情況返回適當的 ActionResult
            //    if (result is UnauthorizedResult)
            //    {
            //        return Unauthorized(); // 返回 401 未授權
            //    }
            //    else if (result is BadRequestObjectResult)
            //    {
            //        return BadRequest(); // 返回 400 錯誤
            //    }
            //}

            //組成訂單編號
            string currentTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            string orderid = /*fUserId.ToString() + */currentTimeString;

            //取快取的token
            //await _jwtService.GetTokenForCurrentUser();

            //if (_memoryCache.TryGetValue(fUserId, out string token))
            //{
            //    //將fUserId存入Dictionary
            //    OrderToken[orderid] = token;
            //}

            // 將 CheckOutDTO 保存到 session 或其他臨時存儲
            //HttpContext.Session.SetString(orderid, JsonConvert.SerializeObject(checkOutDto));

            //組header
            var requestUrl = "/v3/payments/request";
            var nonce = Guid.NewGuid().ToString();
            var requestPayload = new
            {
                amount = checkOutDto.amount, //訂單總額
                currency = "TWD",  //幣值
                orderId = orderid,  //訂單編號
                packages = new[]
                {
                    new {
                        id = orderid,  //一個訂單就付款一次的話這裡就是訂單編號
                        amount = checkOutDto.amount, //相當於訂單總額
                        name = "Japan Activity Memory商品", //訂單描述
                        products = new[]  //訂單明細
                        {
                            new { name = "Japan Activity Memory商品", quantity = 1, price = checkOutDto.amount }
                        }
                    }
                },
                redirectUrls = new
                {
                    //confirmUrl = " https://91ab-2402-7500-487-78a-ad4c-3fde-4c0d-e798.ngrok-free.app/orderconfirmation",  // 支付完成後的回調 URL
                    confirmUrl = " http://localhost:4200/orderconfirmation",  // 支付完成後的回調 URL
                    cancelUrl = "http://localhost:4200/#/home"  // 用戶取消支付的回調 URL
                }
            };

            // 存儲 orderId 與 checkOutDto //將 CheckOutDTO 存入全局的 Dictionary 中
            OrderPayments[orderid] = checkOutDto;



            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + jsonPayload + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature);

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var linePayResponse = JsonConvert.DeserializeObject<PaymentResponseDto>(responseData);

                Console.WriteLine($"Response: {responseData}");  // 檢查 LINE Pay 回應內容
                return Ok(new { paymentUrl = linePayResponse.info.paymentUrl.web });
            }
            else
            {
                // 打印出錯誤訊息以幫助調試
                Console.WriteLine($"Error: {responseData}");

                return BadRequest("Payment request failed");
            }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId)
        {
            var nonce = Guid.NewGuid().ToString();
            var apiUrl = $"/v3/payments/{transactionId}/confirm";

            // 根據 orderId 從全局的 Dictionary 中查找 CheckOutDTO
            if (!OrderPayments.TryGetValue(orderId, out var checkOutDto))
            {
                return BadRequest("Invalid orderId or no checkOutDto found.");
            }

            // 從 session 中取出 CheckOutDTO
            //var checkoutDataJson = HttpContext.Session.GetString(orderId);
            //if (string.IsNullOrEmpty(checkoutDataJson))
            //{
            //    return BadRequest("No checkout data found");
            //}

            //var checkoutDto = JsonConvert.DeserializeObject<CheckOutDTO>(checkoutDataJson);

            //確認金額
            var confirmRequest = new
            {
                amount = checkOutDto.amount,  // 金額應與支付請求中的一致
                currency = "TWD"
            };

            var jsonPayload = JsonConvert.SerializeObject(confirmRequest);
            var signatureString = channelSecretKey + apiUrl + jsonPayload + nonce;
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, signatureString);
            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + apiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature); // 這裡的 Authorization Header 可能需要調整

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                // 支付成功，創建訂單
                // 根據 orderId 從全局的 Dictionary 中查找 CheckOutDTO
                if (!OrderToken.TryGetValue(orderId, out var token))
                {
                    return BadRequest("Invalid orderId or no token found.");
                }
                //await CreateOrder(checkOutDto, token);
                /*Console.WriteLine($"Response: {responseData}");*/  // 檢查 LINE Pay 回應內容
                // 支付確認成功後，從字典中移除訂單資料
                OrderPayments.Remove(orderId);
                OrderToken.Remove(orderId);
                return Redirect("http://localhost:4200/#/home");
            }
            else
            {
                Console.WriteLine($"Error: {responseData}");
                return BadRequest($"Payment confirmation failed: {responseData}");
            }
        }
















        //private readonly LinePayService _linePayService;
        //public LinePayController(LinePayService linePayService)
        //{
        //    _linePayService = linePayService;
        //}

        //[HttpPost("RequestAPI")]
        //public async Task<PaymentResponseDto> RequestAPI(PaymentRequestDto dto)
        //{
        //    return await _linePayService.Request(dto);
        //}

        //[HttpPost("Confirm")]
        //public async Task<PaymentConfirmResponseDto> ConfirmAPI([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        //{
        //    return await _linePayService.Confirm(transactionId, orderId, dto);
        //}












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
