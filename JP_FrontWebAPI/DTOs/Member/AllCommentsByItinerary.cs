namespace JP_FrontWebAPI.DTOs.Member
{
    public class AllCommentsByItinerary
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberPhotoURL { get; set; }
        public int CommentStar { get; set; }
        public string? CommentContent { get; set; }
        public string CommentTime { get; set; }
    }
}
