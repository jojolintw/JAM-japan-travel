using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.OrderViewModels;
using prjJapanTravel_BackendMVC.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class OrderController : Controller
    {
        public JapanTravelContext _context;
        public OrderController(JapanTravelContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            var datas = _context.Orders
                .Select(m => new OrderListViewModel()
                {
                    訂單Id = m.OrderId,
                    訂單編號 = m.OrderNumber,
                    會員 = m.Member.MemberName,
                    //商品名稱 = m.ItineraryDateSystem.ItinerarySystem.ItineraryName,
                    下單時間 = m.OrderTime,
                    付款方式 = m.PaymentMethod.PaymentMethod1,
                    付款時間 = m.PaymentTime,
                    訂單狀態 = m.OrderStatus.OrderStatus1,
                    優惠券 = m.Coupon.CouponName,
                    總金額 = m.TotalAmount
                });

            return View(datas);
        }


        public IActionResult Create()
        {
            ViewBag.MemberList = new SelectList(_context.Members.ToList(), "MemberId", "MemberName");
            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList(), "PaymentMethodId", "PaymentMethod1");
            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList(), "PaymentStatusId", "PaymentStatus1");
            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList(), "OrderStatusId", "OrderStatus1");
            //ViewBag.CouponLIst = new SelectList(_context.Coupons.ToList(), "CouponId", "CouponName" );
            ViewBag.ItineraryList = new SelectList(_context.Itineraries.ToList(), "ItinerarySystemId", "ItineraryName");

            ViewBag.CouponList = _context.Coupons.Select(c => new SelectListItem
            {
                Value = c.CouponId.ToString(),
                Text = $"{c.CouponName} (折扣: {c.Discount}元)"
            }).ToList();

            return View();
        }
    }
}
