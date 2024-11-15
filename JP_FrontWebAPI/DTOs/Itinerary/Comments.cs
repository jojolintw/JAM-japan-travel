namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Comments
    {
        public int ItinerarySystemId { get; set; }
        public int? CommentStar { get; set; }

        public string CommentContent { get; set; }
        public string? CommentDate { get; set; }
    }
}
