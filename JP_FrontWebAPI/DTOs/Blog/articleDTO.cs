using JP_FrontWebAPI.Models;

namespace JP_FrontWebAPI.DTOs.Blog
{
    public class ArticleDTO
    {
        
        //public ArticleDTO() { }
        //public ArticleDTO(ArticleMain articleMain)
        //{
        //    ArticleId = articleMain.ArticleNumber;
        //}

        public int ArticleId { get; set; }
        public int MemberId { get; set; }
        public DateTime LaunchTime { get; set; }
        public string ArticleTitle { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ArticleContent { get; set; }
        public List<string> ArticleHashtags { get; set; }
        // 如果需要，可以取消注释以下行
        // public string Image { get; set; }

    }
}
