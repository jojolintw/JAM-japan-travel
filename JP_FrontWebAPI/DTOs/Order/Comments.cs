using JP_FrontWebAPI.DTOs.Itinerary;
using JP_FrontWebAPI.Models;

namespace JP_FrontWebAPI.DTOs.Order
{
    public class Comments
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int OrderId { get; set; }
        public int ItinerarySystemId { get; set; }
        public int? CommentStar { get; set; }

        public string CommentContent { get; set; }
    }
}
