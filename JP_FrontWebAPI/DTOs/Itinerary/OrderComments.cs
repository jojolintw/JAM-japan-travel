using JP_FrontWebAPI.Models;

namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class OrderComments
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int OrderId { get; set; }
        public List<Comments> Comments { get; set; }
    }
}
