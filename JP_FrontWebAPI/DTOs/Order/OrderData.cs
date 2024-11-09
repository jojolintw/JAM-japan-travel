namespace JP_FrontWebAPI.DTOs.Order
{
    public class OrderData
    {
        public int? couponId { get; set; }
        public string? remarks { get; set; }
        public decimal? totalAmount { get; set; }
        public List<CartItems>? cart { get; set; }
    }
}
