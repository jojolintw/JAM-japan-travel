namespace prjJapanTravel_BackendMVC.ViewModels.BlogViewModels
{
    public class ArticleMainViewModel
    {
        public int 文章編號 { get; set; }

        public int 會員編號 { get; set; }

        public DateTime 文章發布時間 { get; set; }

        public string 文章狀態 { get; set; }

        public string 文章標題 { get; set; }

        public DateTime 文章最後更新時間 { get; set; }

        public string 文章內容 { get; set; }
        public string 文章使用的Hashtag { get; set; }


    }
}
