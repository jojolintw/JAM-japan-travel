namespace prjJapanTravel_BackendMVC.ViewModels.ProductViewModels
{
    public class ImageViewModel
    {
        public string ItinerarySystemId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string ImageDetail { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}