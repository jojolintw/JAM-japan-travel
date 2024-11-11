namespace JP_FrontWebAPI.DTOs.Blog
{
    public class ArticleCreateDTO
    {
        public int MemberId { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public List<int> HashtagNumbers { get; set; }  // 傳遞標籤的 ID 列表
    }
}
