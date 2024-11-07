namespace JP_FrontWebAPI.DTOs.Member
{
    public class LoginMemberDTO
    {
        public int MemberId { get; set; }
        public string ChineseName { get; set; }
        public string? EnglishName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int? CityAreaId { get; set; }
        public string? CityAreaName { get; set; }
        public int? CityId { get; set; }
        
        public string? CityName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public int MemberLevelId { get; set; }
        public string MemberLevel { get; set; }
        public int MemberStatusId { get; set; }
        public string MemberStatus { get; set; }
        public string Photopath { get; set; }
        public IFormFile? photo { get; set; }
    }
}
