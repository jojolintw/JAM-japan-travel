namespace JP_FrontWebAPI.DTOs.Blog
{
    public class articleDTO
    {
        public class ArticleDto
        {
            public int ArticleId { get; set; }
            public int MemberId { get; set; }
            public DateTime LaunchTime { get; set; }
            public string ArticleTitle { get; set; }
            public DateTime LastUpdateTime { get; set; }
            public string ArticleContent { get; set; }
            public List<string> ArticleHashtag { get; set; }
            // 如果需要，可以取消注释以下行
            // public string Image { get; set; }
        }
    }
}
