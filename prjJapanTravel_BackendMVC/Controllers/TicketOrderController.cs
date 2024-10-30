using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.OrderViewModels;
using System.Runtime.Intrinsics.X86;

//namespace prjJapanTravel_BackendMVC.Controllers
//{
//    public class TicketOrderController : Controller
//    {
//        public JapanTravelContext _context;
//        public TicketOrderController(JapanTravelContext context)
//        {
//            _context = context;
//        }
//        public IActionResult List(KeywordViewModel vm)
//        {
//            string keyword = vm.txtKeyword;

//            if (string.IsNullOrEmpty(keyword))
//            {

//                var datas = _context.TicketOrders
//                    .Select(m => new TicketOrderListViewModel()
//                    {
//                        船票訂單編號 = m.TicketOrderId,
//                        訂單編號 = m.TicketOrderNumber,
//                        會員 = m.Member.MemberName,
//                        下單時間 = m.OrderTime,
//                        付款方式 = m.PaymentMethod.PaymentMethod1,
//                        付款狀態 = m.PaymentStatus.PaymentStatus1,
//                        付款時間 = m.PaymentTime,
//                        訂單狀態 = m.OrderStatus.OrderStatus1,
//                        優惠券 = m.Coupon.CouponName,
//                        總金額 = m.TotalAmount
//                    });
//                return View(datas);
//            }
//            else
//            {
//                var datas = _context.TicketOrders
//                    .Where(m=>m.TicketOrderNumber.Contains(keyword)
//                        || m.Member.MemberName.Contains(keyword))
//                    .Select(m => new TicketOrderListViewModel()
//                    {
//                        船票訂單編號 = m.TicketOrderId,
//                        訂單編號 = m.TicketOrderNumber,
//                        會員 = m.Member.MemberName,
//                        下單時間 = m.OrderTime,
//                        付款方式 = m.PaymentMethod.PaymentMethod1,
//                        付款狀態 = m.PaymentStatus.PaymentStatus1,
//                        付款時間 = m.PaymentTime,
//                        訂單狀態 = m.OrderStatus.OrderStatus1,
//                        優惠券 = m.Coupon.CouponName,
//                        總金額 = m.TotalAmount
//                    });
//                return View(datas);
//            }


            
//        }
//        public IActionResult Create()
//        {
//            ViewBag.MemberList = new SelectList(_context.Members.ToList(), "MemberId", "MemberName");
//            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList(), "PaymentMethodId", "PaymentMethod1");
//            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList(), "PaymentStatusId", "PaymentStatus1");
//            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList(), "OrderStatusId", "OrderStatus1");
//            ViewBag.CouponLIst = new SelectList(_context.Coupons.ToList(), "CouponId", "CouponName");


//            return View();
//        }
//        [HttpPost]
//        public IActionResult Create(TicketOrder to)
//        {
//            to.TicketOrderNumber = to.MemberId.ToString() + DateTime.Now.ToString("yyMMddHHmmss");
//            to.OrderTime = DateTime.Now;

//            _context.TicketOrders.Add(to);
//            _context.SaveChanges();
//            return RedirectToAction("List");
//        }
//        public IActionResult Edit(int? id)
//        {
//            var data = _context.TicketOrders
//                .Where(t => t.TicketOrderId == id)
//                .Select(t => new TicketOrderListViewModel()
//                {
//                    船票訂單編號 = t.TicketOrderId,
//                    訂單編號 = t.TicketOrderNumber,
//                    會員編號 = t.MemberId,
//                    會員 = _context.Members
//                        .Where(m => m.MemberId == t.MemberId)
//                        .Select(m=>m.MemberName)
//                        .FirstOrDefault(),
//                    下單時間 = t.OrderTime,
//                    付款方式編號 = t.PaymentMethodId,
//                    付款狀態編號 = t.PaymentStatusId,
//                    付款時間 = t.PaymentTime,
//                    訂單狀態編號 = t.OrderStatusId,
//                    優惠券編號 = t.CouponId,
//                    優惠券 = _context.Coupons
//                        .Where(c => c.CouponId == t.CouponId)
//                        .Select(c => c.CouponName)
//                        .FirstOrDefault(),
//                    總金額 = t.TotalAmount,
//                }).FirstOrDefault();

//            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList()
//                , "PaymentStatusId", "PaymentStatus1", data.付款狀態編號);
//            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList()
//                , "PaymentMethodId", "PaymentMethod1", data.付款方式編號);
//            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList()
//                , "OrderStatusId", "OrderStatus1", data.訂單狀態編號);

//            return View(data);
//        }

//        [HttpPost]
//        public IActionResult Edit(TicketOrderListViewModel vm)
//        {
//            var data = _context.TicketOrders
//                .FirstOrDefault(to => to.TicketOrderId == vm.船票訂單編號);
//            if (data == null)
//                return RedirectToAction("List");
//            data.TicketOrderId = (int)vm.船票訂單編號;
//            data.TicketOrderNumber = vm.訂單編號;
//            //data.MemberId = (int)vm.會員編號;
//            //data.OrderTime = vm.下單時間;
//            data.PaymentMethodId = (int)vm.付款方式編號;
//            data.PaymentStatusId = (int)vm.付款狀態編號;
//            data.PaymentTime = vm.付款時間;
//            data.OrderStatusId = (int)vm.訂單狀態編號;
//            //data.CouponId = (int?)vm.優惠券編號;
//            data.TotalAmount = vm.總金額;

//            _context.SaveChanges();
//            return RedirectToAction("List");
//        }

//        public IActionResult Cancel(int? id)
//        {
//            var data = _context.TicketOrders.FirstOrDefault(io => io.TicketOrderId== id);
//            if (data != null)
//            {
//                data.OrderStatusId = 3;
//                _context.SaveChanges();
//            }
//            return RedirectToAction("List");
//        }

//        public IActionResult Details(int? id)
//        {
//            var datas = _context.TicketOrderItems
//                .Where(t => t.TicketOrderId == id)
//                .Select(t => new TicketOrderDetailViewModel()
//                {
//                    會員 = t.TicketOrder.Member.MemberName,
//                    訂單編號 = t.TicketOrder.TicketOrderNumber,
//                    出發地點 = t.Schedule.Route.OriginPort.PortName,
//                    抵達地點 = t.Schedule.Route.DestinationPort.PortName,
//                    航線描述 = t.Schedule.Route.RouteDescription,
//                    數量 = t.Quantity,
//                    單價 = (decimal) t.Schedule.Route.Price,
//                    總金額 = (decimal)t.Schedule.Route.Price * t.Quantity
//                            - (t.TicketOrder.Coupon.Discount != null ? t.TicketOrder.Coupon.Discount : 0),
//                    優惠券名稱 = t.TicketOrder.Coupon.CouponName != null ? t.TicketOrder.Coupon.CouponName : "未使用優惠券",
//                    優惠金額 = t.TicketOrder.Coupon.Discount != null ? t.TicketOrder.Coupon.Discount : 0,
//                }).ToList();
//            if (!datas.Any())
//            {
//                return View(datas);
//            }
//            ViewBag.MemberName = datas.First().會員;
//            ViewBag.OrderNumber = datas.First().訂單編號;
            

//            return View(datas);
//        }
//    }
//}
