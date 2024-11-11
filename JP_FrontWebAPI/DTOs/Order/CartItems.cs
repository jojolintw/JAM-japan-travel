namespace JP_FrontWebAPI.DTOs.Order
{
    public class CartItems
    {
        public int? itineraryDateSystemId {get; set;}
        public int? itinerarySystemId { get; set;}
        public string? name { get; set;}
        public decimal? price { get; set;}
        public int? quantity { get; set;}
        public string? imagePath { get; set;}

    }
}
