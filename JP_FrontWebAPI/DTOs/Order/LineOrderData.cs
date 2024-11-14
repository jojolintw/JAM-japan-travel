namespace JP_FrontWebAPI.DTOs.Order
{
    public class LineOrderData
    {
        public string orderId { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string productName {  get; set; }
        public string confirmUrl {  get; set; }
        public string cancelUrl {  get; set; }

    }
}
