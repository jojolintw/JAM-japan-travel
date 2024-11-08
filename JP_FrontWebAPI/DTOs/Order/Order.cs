namespace JP_FrontWebAPI.DTOs.Order
{
    public class Order
    {
        public int? couponId { get; set; }
        public string? remarks { get; set; }
        public List<CartItems>? items { get; set; }
    }
}
