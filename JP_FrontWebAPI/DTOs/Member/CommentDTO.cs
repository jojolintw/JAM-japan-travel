namespace JP_FrontWebAPI.DTOs.Member
{
    public class CommentDTO
    {
        public int ordertetailId { get; set; }
        public int itinerarySystemId { get; set; }
        public string itineraryName { get; set; }
        public int? CommentStar { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? CommentTime { get; set; }

    }
}
