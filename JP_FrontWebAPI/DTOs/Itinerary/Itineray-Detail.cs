using Microsoft.AspNetCore.Http;

namespace JP_FrontWebAPI.DTOs.DTOs.Itinerary
{
    public class Itineray_Detail
    {
      public int ItinerarySystemId { get; set; }
      public string ItineraryName { get; set; }
      public string AreaName { get; set; }
      public string ThemeName { get; set; }
      public string ActivityName { get; set; }
      public List<string> ImagePath { get; set; }
      public List<string> ItineraryDates { get; set; }
      public int Stock {  get; set; }
      public decimal Price { get; set; }
      public string? ItineraryDetail {  get; set; }
      public string? ItineraryBrief {  get; set; }
      public string? ItineraryNote {  get; set; }
    }
}
