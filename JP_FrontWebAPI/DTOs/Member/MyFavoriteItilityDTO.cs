namespace JP_FrontWebAPI.DTOs.Member
{
    public class MyFavoriteItilityDTO
    {
        public int ItinerarySystemId { get; set; }

        public string ItineraryName { get; set; }

        public int AreaSystemId { get; set; }

        public decimal Price { get; set; }
        public List<string> DepartureDate { get; set; }
        public string ItineraryDetail { get; set; }

        public string Image { get; set; }
    }
}
