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
        //public List<HashTag> HashTags { get; set; } = new List<HashTag>();
        public string MemberName { get; set; }
        public string? ImagePath { get; set; }

        // 新增：用來儲存提取的 Base64 圖片
        public List<string> ArticleImages { get; set; } = new List<string>();
    }


    public class HashTag
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = default;
    }

}
