namespace prjJapanTravel_BackendMVC.ViewModels.BlogViewModels
{
    public class ArticleMainViewModel
    {
        public int 文章編號 { get; set; }

        public int 會員編號 { get; set; }

        public DateTime 文章發布時間 { get; set; }

        public int 文章狀態編號 { get; set; }

        public string 文章標題 { get; set; }

        public DateTime 文章更新時間 { get; set; }
    }
}
