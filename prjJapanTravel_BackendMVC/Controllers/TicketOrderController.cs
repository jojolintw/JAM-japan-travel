using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.OrderViewModels;
using System.Runtime.Intrinsics.X86;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class TicketOrderController : Controller
    {
        public JapanTravelContext _context;
        public TicketOrderController(JapanTravelContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            var datas = _context.TicketOrders
                .Select(m => new TicketOrderListViewModel()
                {
                    船票訂單編號 = m.TicketOrderId,
                    訂單編號 = m.TicketOrderNumber,
                    會員 = m.Member.MemberName,
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
        public IActionResult Create(TicketOrder to)
        {
            _context.TicketOrders.Add(to);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {
            var data = _context.TicketOrders
                .Where(t => t.TicketOrderId == id)
                .Select(t => new TicketOrderListViewModel()
                {
                    船票訂單編號 = t.TicketOrderId,
                    訂單編號 = t.TicketOrderNumber,
                    會員編號 = t.MemberId,
                    會員 = _context.Members
                        .Where(m => m.MemberId == t.MemberId)
                        .Select(m=>m.MemberName)
                        .FirstOrDefault(),
                    下單時間 = t.OrderTime,
                    付款方式編號 = t.PaymentMethodId,
                    付款狀態編號 = t.PaymentStatusId,
                    付款時間 = t.PaymentTime,
                    訂單狀態編號 = t.OrderStatusId,
                    優惠券編號 = t.CouponId,
                    優惠券 = _context.Coupons
                        .Where(c => c.CouponId == t.CouponId)
                        .Select(c => c.CouponName)
                        .FirstOrDefault(),
                    總金額 = t.TotalAmount,
                }).FirstOrDefault();

            ViewBag.PaymentStatusList = new SelectList(_context.PaymentStatuses.ToList()
                , "PaymentStatusId", "PaymentStatus1", data.付款狀態編號);
            ViewBag.PaymentMethodList = new SelectList(_context.PaymentMethods.ToList()
                , "PaymentMethodId", "PaymentMethod1", data.付款方式編號);
            ViewBag.OrderStatusList = new SelectList(_context.OrderStatuses.ToList()
                , "OrderStatusId", "OrderStatus1", data.訂單狀態編號);
            //ViewBag.CouponList = new SelectList(_context.Coupons.ToList()
            //    , "CouponId", "CouponName", data.優惠券編號);
            //ViewBag.MemberList = new SelectList(_context.Members.ToList()
            //    , "MemberId", "MemberName", data.會員編號);

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(TicketOrderListViewModel vm)
        {
            var data = _context.TicketOrders
                .FirstOrDefault(to => to.TicketOrderId == vm.船票訂單編號);
            if (data == null)
                return RedirectToAction("List");
            data.TicketOrderId = (int)vm.船票訂單編號;
            data.TicketOrderNumber = vm.訂單編號;
            //data.MemberId = (int)vm.會員編號;
            data.OrderTime = vm.下單時間;
            data.PaymentMethodId = (int)vm.付款方式編號;
            data.PaymentStatusId = (int)vm.付款狀態編號;
            data.PaymentTime = vm.付款時間;
            data.OrderStatusId = (int)vm.訂單狀態編號;
            //data.CouponId = (int?)vm.優惠券編號;
            data.TotalAmount = vm.總金額;

            _context.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Cancel(int? id)
        {
            var data = _context.TicketOrders.FirstOrDefault(io => io.TicketOrderId== id);
            if (data != null)
            {
                data.OrderStatusId = 3;
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
