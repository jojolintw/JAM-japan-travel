namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Theme_Activity
    {
        public int ThemeSystemId { get; set; }
        public string ThemeName { get; set; }
        public List <Activity> Activities { get; set; }
    }
}