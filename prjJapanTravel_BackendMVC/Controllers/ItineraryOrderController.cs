using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.OrderViewModels;
using prjJapanTravel_BackendMVC.ViewModels.ProductViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ItineraryOrderController : Controller
    {
        public JapanTravelContext _context;
        public ItineraryOrderController(JapanTravelContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var datas = _context.ItineraryOrders
                .Include(o => o.Member)
                //.Include(o => o.ItineraryDateSystem)
                //    .ThenInclude(oi => oi.ItinerarySystemId)
                .Include(o => o.PaymentMethod)
                .Include(o => o.PaymentStatus)
                .Include(o => o.Coupon)
                .Select(m => new ItineraryOrderListViewModel()
            {
                行程訂單編號 = m.ItineraryOrderId,
                訂單編號 = m.ItineraryOrderNumber,
                會員 = m.Member.MemberName,
                //行程編號 = m.ItineraryDateSystemId,
                //數量 = m.Quantity,
                下單時間 = m.OrderTime,
                付款方式 = m.PaymentMethod.PaymentMethod1,
                付款狀態 = m.PaymentStatus.PaymentStatus1,
                訂單狀態 = m.OrderStatus.OrderStatus1,
                優惠券 = m.Coupon.CouponName,
                總金額 = m.TotalAmount
            });
            return View(datas);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ItineraryOrder io)
        {
            _context.ItineraryOrders.Add(io);
            _context.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            var data = _context.ItineraryOrders
                .Include(io => io.Member)
                .FirstOrDefault(io => io.ItineraryOrderId == id);
            if (data == null)
                return RedirectToAction("List");

            //ViewBag.MemberName = 

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Cancel(int? id)
        {
            var data = _context.ItineraryOrders.FirstOrDefault(io => io.ItineraryOrderId == id);
            if (data != null)
            {
                data.OrderStatusId = 3;
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
