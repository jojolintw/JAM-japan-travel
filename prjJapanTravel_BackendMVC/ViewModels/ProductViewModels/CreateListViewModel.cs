using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ProductViewModels
{
    public class CreateListViewModel
    {
        public List<Area> areaList {  get; set; }
        public List<Activity> activityList { get; set; }
        public List<Theme> themeList { get; set; }
        public List<ImageViewModel> imageViewModel { get; set; }
    }
}
