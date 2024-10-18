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

        public IActionResult List(KeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;

            if (string.IsNullOrEmpty(keyword))
            {
                var datas = _context.ItineraryOrders
                    .Select(m => new ItineraryOrderListViewModel()
                    {
                        行程訂單編號 = m.ItineraryOrderId,
                        訂單編號 = m.ItineraryOrderNumber,
                        會員 = m.Member.MemberName,
                        行程名稱 = m.ItineraryDateSystem.ItinerarySystem.ItineraryName,
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
            else
            {
                var datas = _context.ItineraryOrders
                    .Where(m=>m.ItineraryOrderNumber.Contains(keyword)
                        ||m.Member.MemberName.Contains(keyword))
                    .Select(m => new ItineraryOrderListViewModel()
                    {
                        行程訂單編號 = m.ItineraryOrderId,
                        訂單編號 = m.ItineraryOrderNumber,
                        會員 = m.Member.MemberName,
                        行程名稱 = m.ItineraryDateSystem.ItinerarySystem.ItineraryName,
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
                
        }


        public IActionResult Create()
        {
            ViewBag.MemberList = new SelectList(_context.Members.ToList(), "MemberId", "MemberName");
            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList(), "PaymentMethodId", "PaymentMethod1");
            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList(), "PaymentStatusId", "PaymentStatus1");
            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList(), "OrderStatusId", "OrderStatus1");
            ViewBag.CouponLIst = new SelectList(_context.Coupons.ToList(), "CouponId", "CouponName");
            ViewBag.ItineraryList = new SelectList(_context.Itineraries.ToList(), "ItinerarySystemId", "ItineraryName");


            return View();
        }


        [HttpPost]
        public IActionResult Create(ItineraryOrder io)
        {
            io.ItineraryOrderNumber = io.MemberId.ToString() + DateTime.Now.ToString("yyMMddHHmmss");
            io.OrderTime = DateTime.Now;
            _context.ItineraryOrders.Add(io);
            _context.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult GetItineraryDates(int id)
        {
            var dates = _context.ItineraryDates
                .Where(d => d.ItinerarySystemId == id)
                .Select(d => new
                {
                    dateId = d.ItineraryDateSystemId,
                    date = d.DepartureDate // 根據實際的日期屬性進行調整
                }).ToList();

            return Json(dates);
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
                    會員 = _context.Members
                        .Where(m => m.MemberId == i.MemberId)
                        .Select(m =>m.MemberName)
                        .FirstOrDefault(),
                    行程編號 = i.ItineraryDateSystemId,
                    //行程名稱 = _context.Itineraries
                    //    .Include("Itinerary")
                    //    .Where(it=>it.ItinerarySystemId == i.ItineraryDateSystem.ItinerarySystemId)
                    //    .Select(it => it.ItineraryName)
                    //    .FirstOrDefault(),
                    數量 = i.Quantity,
                    下單時間 = i.OrderTime,
                    付款方式編號 = i.PaymentMethodId,
                    付款狀態編號 = i.PaymentStatusId,
                    付款時間 = i.PaymentTime,
                    訂單狀態編號 = i.OrderStatusId,
                    優惠券編號 = i.CouponId,
                    優惠券 = _context.Coupons
                        .Where(c => c.CouponId == i.CouponId)
                        .Select(c => c.CouponName)
                        .FirstOrDefault(),
                    總金額 = i.TotalAmount,
                }).FirstOrDefault();

            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList()
                , "PaymentStatusId", "PaymentStatus1", data.付款狀態編號);
            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList()
                , "PaymentMethodId", "PaymentMethod1", data.付款方式編號);
            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList()
                , "OrderStatusId", "OrderStatus1", data.訂單狀態編號);

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(ItineraryOrderListViewModel vm)
        {
            var data = _context.ItineraryOrders
                .FirstOrDefault(io => io.ItineraryOrderId == vm.行程訂單編號);
            if (data == null)
                return RedirectToAction("List");
            data.ItineraryOrderId = (int)vm.行程訂單編號;
            data.ItineraryOrderNumber = vm.訂單編號;
            //data.MemberId = (int)vm.會員編號;
            data.ItineraryDateSystemId = (int)vm.行程編號;
            data.Quantity = (int)vm.數量;
            //data.OrderTime = vm.下單時間;
            data.PaymentMethodId = (int)vm.付款方式編號;
            data.PaymentStatusId = (int)vm.付款狀態編號;
            data.PaymentTime = vm.付款時間;
            data.OrderStatusId = (int)vm.訂單狀態編號;
            //data.CouponId = (int?)vm.優惠券編號;
            data.TotalAmount = vm.總金額;

            _context.SaveChanges();
            return RedirectToAction("List");



        }

        public IActionResult Delete(int? id)
        {
            var data = _context.ItineraryOrders.FirstOrDefault(io => io.ItineraryOrderId == id);
            if (data != null)
            {
                _context.Remove(data);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult Details(int? id)
        {
            var datas = _context.ItineraryOrders
                .Where(i => i.ItineraryOrderId == id)
                .Select(i => new ItineraryOrderDetailViewModel()
                {
                    會員 = i.Member.MemberName,
                    訂單編號 = i.ItineraryOrderNumber,
                    行程名稱 = i.ItineraryDateSystem.ItinerarySystem.ItineraryName,
                    日期 = i.ItineraryDateSystem.DepartureDate,
                    行程須知 = i.ItineraryDateSystem.ItinerarySystem.ItineraryDetail,
                    數量 = i.Quantity,
                    單價 = (decimal)i.ItineraryDateSystem.ItinerarySystem.Price,
                    總金額 = (decimal)i.ItineraryDateSystem.ItinerarySystem.Price * i.Quantity 
                            - (i.Coupon.Discount != null ? i.Coupon.Discount : 0),
                    優惠券名稱 = i.Coupon.CouponName != null ? i.Coupon.CouponName : "未使用優惠券",
                    優惠金額 = i.Coupon.Discount != null ? i.Coupon.Discount : 0,
                }).ToList();
            ViewBag.MemberName = datas.First().會員;
            ViewBag.OrderNumber = datas.First().訂單編號;

            return View(datas);
        }
    }
}
