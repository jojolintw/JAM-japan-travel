using JP_FrontWebAPI.Models;

namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Itineray_Detail
    {
      public int ItinerarySystemId { get; set; }
      public string ItineraryName { get; set; }
      public string AreaName { get; set; }
      public string ThemeName { get; set; }
      public string ActivityName { get; set; }
      public int ActivityId { get; set; }
      public List<string> ImagePath { get; set; }
      public List<ItineraryDate> ItineraryBatch { get; set; }
      public decimal Price { get; set; }
      public string? ItineraryDetail {  get; set; }
      public string? ItineraryBrief {  get; set; }
      public string? ItineraryNote {  get; set; }
    }
}
