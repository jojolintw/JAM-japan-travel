namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Itinerary_List
    {
        public int ItinerarySystemId { get; set; }
        public string? ItineraryName { get; set; }
        public List<string> ItineraryDate { get; set; }
        public int ActivityId { get; set; }
        public string? AreaName { get; set; }
        public string? ImagePath { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
    }
}
