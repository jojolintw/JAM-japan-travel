using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace prjJapanTravel_BackendMVC.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int 會員編號 { get; set; }
        public string 會員姓名 { get; set; }
        public bool 性別 { get; set; }
        public DateTime 生日 { get; set; }
        public int 城市編號 { get; set; }
        public string 城市 { get; set; }
        public string 手機號碼 { get; set; }
        public string Email { get; set; }
        public string 密碼 { get; set; }
        public int 會員等級編號 { get; set; }
        public string 會員等級 { get; set; }
        public int 會員狀態編號 { get; set; }
        public string 會員狀態 { get; set; }
        public string 頭像路徑 { get; set; }
        public IFormFile? photo { get; set; }
    }
}
