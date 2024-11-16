using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JP_FrontWebAPI.Common;
using System.Text;
using static JP_FrontWebAPI.DTOs.Order.LinepayDTO;



namespace JP_FrontWebAPI.Service
{
    public class LinePayService/* : ILinePayService*/
    {
        private readonly string channelId = "2006530351";  // 替換為您的 Channel ID
        private readonly string channelSecretKey = "96f3c66527b68f45c7dee92962c58855";  // 替換為您的 Channel Secret
        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";  // 可選，如果沒有設定可省略
        private readonly JsonProvider _jsonProvider;
        private HttpClient client;
        //private readonly string apiUrl = "https://sandbox-api-pay.line.me/v3/payments/request";  // 沙盒環境 API
        public LinePayService()
        {
            client = new HttpClient();
            _jsonProvider = new JsonProvider();
        }


        //public async Task<PaymentResponseDto> Request(PaymentRequestDto dto)
        //{
        //    PaymentResponseDto linePayResponse;
        //    var json = _jsonProvider.Serialize(dto);
        //    // 產生 GUID Nonce
        //    var nonce = Guid.NewGuid().ToString();
        //    // 要放入 signature 中的 requestUrl
        //    var requestUrl = "/v3/payments/request";

        //    //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
        //    var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

        //    using (HttpClient client = new())
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
        //        {
        //            Content = new StringContent(json, Encoding.UTF8, "application/json")
        //        };
        //        // 帶入 Headers
        //        client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

        //        var response = await client.SendAsync(request);
        //        var resultString = await response.Content.ReadAsStringAsync();
        //        linePayResponse = _jsonProvider.Deserialize<PaymentResponseDto>(resultString);
        //        linePayResponse.Info.StringTransactionId = linePayResponse.Info.TransactionId.ToString();
        //    }

        //    return linePayResponse;
        //}


        //public async Task<PaymentConfirmResponseDto> Confirm(string transactionId, string orderId, PaymentConfirmDto dto)
        //{
        //    PaymentConfirmResponseDto result;

        //    var json = _jsonProvider.Serialize(dto);

        //    using (HttpClient client = new())
        //    {
        //        var nonce = Guid.NewGuid().ToString();
        //        var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
        //        var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);
        //        var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
        //        {
        //            Content = new StringContent(json, Encoding.UTF8, "application/json")
        //        };
        //        client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

        //        var response = await client.SendAsync(request);
        //        result = _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
        //    }
        //    return result;
        //}





























        //    // 生成隨機 Nonce
        //    private string GenerateNonce()
        //    {
        //        return Guid.NewGuid().ToString("N");  // 使用 GUID 生成唯一的 nonce
        //    }

        //    // 生成簽名
        //    private string GenerateSignature(string body, string nonce)
        //    {
        //        var key = Encoding.UTF8.GetBytes(_channelSecret);
        //        var message = $"{nonce}\n{body}";

        //        using (var hmacsha256 = new HMACSHA256(key))
        //        {
        //            byte[] signatureBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
        //            return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();  // 轉為小寫的十六進位字串
        //        }
        //    }

        //    // 發送請求創建訂單
        //    public async Task<string> CreateOrder(decimal amount)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string nonce = GenerateNonce();  // 生成 nonce
        //            var orderData = new
        //            {
        //                amount = amount,
        //                currency = "TWD",
        //                orderId = "jam202411110942",  // 您可以根據需求設置您的訂單ID
        //                packages = new[]
        //                {
        //                new
        //                {
        //                    id = "1",
        //                    amount = amount,
        //                    products = new[]
        //                    {
        //                        new
        //                        {
        //                            id = "1",
        //                            name = "行程1",
        //                            imageUrl = "https://pay-store.example.com/images/pen_brown.jpg",
        //                            quantity = 2,
        //                            price = 50
        //                        }
        //                    }
        //                }
        //            },
        //                redirectUrls = new
        //                {
        //                    confirmUrl = "https://pay-store.example.com/order/payment/authorize",
        //                    cancelUrl = "https://pay-store.example.com/order/payment/cancel"
        //                }
        //            };

        //            string body = JsonConvert.SerializeObject(orderData);  // 將訂單資料序列化為 JSON 字串
        //            string signature = GenerateSignature(body, nonce);  // 根據 body 和 nonce 生成簽名

        //            // 設定 HTTP 請求標頭
        //            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        //            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", _channelId);
        //            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
        //            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
        //            client.DefaultRequestHeaders.Add("X-LINE-MerchantDeviceProfileId", _merchantDeviceProfileId);


        //            var content = new StringContent(body, Encoding.UTF8, "application/json");

        //            // 發送 POST 請求
        //            var response = await client.PostAsync(_apiUrl, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseData = await response.Content.ReadAsStringAsync();
        //                return responseData;  // 返回回應資料（可處理顯示付款頁面或錯誤訊息）
        //            }
        //            else
        //            {
        //                return $"Error: {response.StatusCode}";
        //            }
        //        }
        //    }
        //}



        //    public async Task<string> ConfirmOrder(string transactionId)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", _channelId);
        //            client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", _channelSecret);

        //            var confirmData = new
        //            {
        //                transactionId = transactionId,
        //                amount = 1000, // 金額（需要與創建訂單時一致）
        //                currency = "TWD"
        //            };

        //            var content = new StringContent(JsonConvert.SerializeObject(confirmData), Encoding.UTF8, "application/json");

        //            var response = await client.PostAsync("https://sandbox-api-pay.line.me/v3/payment/confirm", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseData = await response.Content.ReadAsStringAsync();
        //                // 在這裡您可以處理回應，例如：付款成功處理
        //                Console.WriteLine(responseData);
        //                return responseData;
        //            }
        //            else
        //            {
        //                return $"Error: {response.StatusCode}";
        //            }
        //        }
        //    }
        //}
    }
}
