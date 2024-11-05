using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace JP_FrontWebAPI.Service
{
    public class LinePayService
    {
        private readonly string channelId = "2006530351";  // LINE Pay Channel ID
        private readonly string channelSecret = "LINE Pay Channel Secret";  // LINE Pay Channel Secret
        private readonly string apiUrl = "https://sandbox-api-pay.line.me/v3/payments/request";  // Sandbox API URL

        public async Task<string> RequestPayment(decimal amount, string orderId)
        {
            var requestBody = new
            {
                amount = amount,
                currency = "TWD",
                orderId = orderId,
                packages = new[]
                {
                new
                {
                    id = orderId,
                    amount = amount,
                    name = "旅遊行程",
                    products = new[]
                    {
                        new { name = "產品名稱", quantity = 1, price = amount }
                    }
                }
            },
                redirectUrls = new
                {
                    confirmUrl = "https://yourwebsite.com/confirm",
                    cancelUrl = "https://yourwebsite.com/cancel"
                }
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
