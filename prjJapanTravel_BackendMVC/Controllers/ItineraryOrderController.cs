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
                //.Include(o => o.Member)
                //.Include(o => o.ItineraryDateSystem)
                //    .ThenInclude(oi => oi.ItinerarySystem)
                //.Include(o => o.PaymentMethod)
                //.Include(o => o.PaymentStatus)
                //.Include(o => o.Coupon)
                .Select(m => new ItineraryOrderListViewModel()
                {
                行程訂單編號 = m.ItineraryOrderId,
                訂單編號 = m.ItineraryOrderNumber,
                會員 = m.Member.MemberName,
                行程名稱 = m.ItineraryDateSystem.ItinerarySystem.ItineraryName,
                //數量 = m.Quantity,
                下單時間 = m.OrderTime,
                付款方式 = m.PaymentMethod.PaymentMethod1,
                付款狀態 = m.PaymentStatus.PaymentStatus1,
                付款時間 = m.PaymentTime,
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
                .Where(i => i.ItineraryOrderId == id)
                .Select(i => new ItineraryOrderListViewModel()
                {
                    行程訂單編號 = i.ItineraryOrderId,
                    訂單編號 = i.ItineraryOrderNumber,
                    會員編號 = i.MemberId,
                    行程編號 = i.ItineraryDateSystemId,
                    數量 = i.Quantity,
                    下單時間 = i.OrderTime,
                    付款方式編號 = i.PaymentMethodId,
                    付款狀態編號 = i.PaymentStatusId,
                    付款時間 = i.PaymentTime,
                    訂單狀態編號 = i.OrderStatusId,
                    優惠券編號 = i.CouponId,
                    總金額 = i.TotalAmount,
                }).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(ItineraryOrderListViewModel vm)
        {
            var data = _context.ItineraryOrders
                .Include(io => io.Member)
                .Include(io => io.PaymentMethod)
                .Include(io => io.PaymentStatus)
                .Include(io => io.OrderStatus)
                .Include(io => io.Coupon)
                .Include(io => io.ItineraryDateSystem)
                    .ThenInclude(io => io.ItinerarySystem)
                .FirstOrDefault(io => io.ItineraryOrderId == vm.行程訂單編號);
            if (data == null)
                return RedirectToAction("List");
            data.ItineraryOrderId = (int)vm.行程訂單編號;
            data.ItineraryOrderNumber = vm.訂單編號;
            data.MemberId = (int)vm.會員編號;
            data.ItineraryDateSystemId = (int)vm.行程編號;
            data.Quantity = (int)vm.數量;
            data.OrderTime = vm.下單時間;
            data.PaymentMethodId = (int)vm.付款方式編號;
            data.PaymentStatusId = (int)vm.付款狀態編號;
            data.PaymentTime = vm.付款時間;
            data.OrderStatusId = (int)vm.訂單狀態編號;
            data.CouponId = (int?)vm.優惠券編號;
            data.TotalAmount = vm.總金額;

            _context.SaveChanges();
            return RedirectToAction("List");



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
        public IActionResult Detail(int? id)
        {
            return View();
        }
    }
}
