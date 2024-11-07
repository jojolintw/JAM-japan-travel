namespace JP_FrontWebAPI.DTOs.Member
{
    public class AlterMemberDTO
    {
        public string MemberName { get; set; }
        public string? EnglishName { get; set; }
        public string? Gender { get; set; }
        public string? Birthday { get; set; }
        public int? CityId { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        //public IFormFile Memberphoto { get; set; }
    }
}
