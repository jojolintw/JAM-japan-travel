using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.ProductViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ProductController : Controller
    {
        private JapanTravelContext _JP;
        public ProductController(JapanTravelContext JP)
        {
            _JP = JP;
        }
        public IActionResult List()
        {
            var datas = _JP.Itineraries.Select(n => new ItineraryListViewModel()
            {
                行程系統編號 = n.ItinerarySystemId,
                行程編號 = n.ItineraryId,
                行程名稱 = n.ItineraryName,
                體驗項目 = n.ActivitySystem.ActivityName,
                總團位 = n.Stock,
                價格 = n.Price,
                體驗主題 = n.ThemeSystem.ThemeName,
                地區 = n.AreaSystem.AreaName,
                行程日期 = n.ItineraryDates.Where(date=>date.ItinerarySystemId == n.ItinerarySystemId).ToList(),
                行程圖片 = n.Images.Select(img => new ImageViewModel
                {
                    ImagePath = img.ImagePath,
                    ImageName = img.ImageName
                }).ToList(),
                行程詳情 = n.ItineraryDetail,
                行程簡介 = n.ItineraryBrief,
                行程注意事項 = n.ItineraryNotes
            });
            return View(datas);
        }

        public IActionResult ItineraryCreate()
        {
            CreateListViewModel c = new CreateListViewModel();
            c.areaList = _JP.Areas.ToList();
            c.activityList = _JP.Activities.ToList();
            return View(c);
        }

        [HttpPost]
        public IActionResult ItineraryCreate( ItineraryListViewModel itimodel, List<IFormFile> ItineraryPics)
        {
            Itinerary itinerary = new Itinerary();
            
            itinerary.ItineraryId = itimodel.行程編號;
            itinerary.ItineraryName = itimodel.行程名稱;
            itinerary.ActivitySystemId = itimodel.ActivitySystem.ActivitySystemId;
            itinerary.Stock = itimodel.總團位;
            itinerary.Price = itimodel.價格;
            //itinerary.ThemeSystemId = itimodel.ThemeSystem.ThemeSystemId;
            itinerary.AreaSystemId = itimodel.AreaSystem.AreaSystemId;
            itinerary.ItineraryDetail = itimodel.行程詳情;
            itinerary.ItineraryBrief = itimodel.行程簡介;
            itinerary.ItineraryNotes = itimodel.行程注意事項;

            _JP.Itineraries.Add(itinerary);
            _JP.SaveChanges();

            if (itimodel.行程日期 != null && itimodel.行程日期.Count > 0)
            {
                foreach (var date in itimodel.行程日期)
                {
                    ItineraryDate itineraryDate = new ItineraryDate
                    {
                        ItinerarySystemId = itinerary.ItinerarySystemId, // 使用新建行程的 ItinerarySystemId
                        DepartureDate = date.DepartureDate // 來自於 ViewModel 的行程日期
                    };
                    _JP.ItineraryDates.Add(itineraryDate); // 將出發日期添加到上下文中
                }
                _JP.SaveChanges(); // 將所有出發日期保存到資料庫
            }

            if (itimodel.ItineraryPics != null && ItineraryPics.Count > 0)
            {
                for (int i = 0; i < ItineraryPics.Count; i++)
                {
                    var file = ItineraryPics[i];
                    if (file.Length > 0)
                    {
                        // 生成唯一的檔名
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        // 圖片的實際伺服器路徑
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product", uniqueFileName);

                        // 儲存圖片到指定路徑
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // 獲取圖片名稱和描述
                        var imageViewModel = itimodel.行程圖片[i];

                        // 新增圖片資料到 Image 表
                        Image itineraryImage = new Image()
                        {
                            ItinerarySystemId = itinerary.ItinerarySystemId,
                            ImageName = imageViewModel.ImageName,  // 確保圖片名稱對應
                            ImagePath = $"/images/Product/{uniqueFileName}",  // 相對路徑
                            ImageDetail = imageViewModel.ImageDetail  // 圖片描述
                        };

                        _JP.Images.Add(itineraryImage);  // 添加圖片資料到資料庫
                    }
                }
            }
           
            return RedirectToAction("List");
        }

        public IActionResult ItineraryDelete(int? id)
        {
            Itinerary iti = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == id);
            if (iti != null)
            {
                var itineraryTimes = _JP.ItineraryDates.Where(n => n.ItinerarySystemId == id).ToList();
                var itineraryImages = _JP.Images.Where(n => n.ItinerarySystemId == id).ToList();

                _JP.ItineraryDates.RemoveRange(itineraryTimes);
                _JP.Images.RemoveRange(itineraryImages);
                _JP.Itineraries.Remove(iti);

                _JP.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult ItineraryEdit(int? id)
        {
            var data = _JP.Itineraries.Where(n => n.ItinerarySystemId == id).Select(n => new ItineraryListViewModel()
            {
                行程系統編號 = n.ItinerarySystemId,
                行程編號 = n.ItineraryId,
                行程名稱 = n.ItineraryName,
                體驗項目編號 = n.ActivitySystemId,
                體驗項目 = n.ActivitySystem.ActivityName,
                總團位 = n.Stock,
                價格 = n.Price,
                體驗主題編號 = n.ThemeSystemId,
                體驗主題 = n.ThemeSystem.ThemeName,
                地區編號 = n.AreaSystemId,
                地區 = n.AreaSystem.AreaName,
                //行程圖片 = n.ItineraryPicSystem.ImagePath,
                行程詳情 = n.ItineraryDetail,
                行程簡介 = n.ItineraryBrief,
                行程注意事項 = n.ItineraryNotes
            }).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public IActionResult ItineraryEdit(ItineraryListViewModel itiModel)
        {
            var itinerary = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == itiModel.行程系統編號);
            if (itinerary == null)
            {
                return RedirectToAction("List");
            }

            itinerary.ItineraryId = itiModel.行程編號;
            itinerary.ItineraryName = itiModel.行程名稱;
            itinerary.ActivitySystemId = itiModel.體驗項目編號;
            itinerary.Stock = itiModel.總團位;
            itinerary.Price = itiModel.價格;
            itinerary.ThemeSystemId = itiModel.體驗主題編號;
            itinerary.AreaSystemId = itiModel.地區編號;
            //itinerary.ItineraryPicSystemId = itiModel.行程圖片編號;
            itinerary.ItineraryDetail = itiModel.行程詳情;
            itinerary.ItineraryBrief = itiModel.行程簡介;
            itinerary.ItineraryNotes = itiModel.行程注意事項;

            _JP.SaveChanges();
            return RedirectToAction("List");
            
        }
    }
}
