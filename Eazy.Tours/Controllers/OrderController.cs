using Eazy.Tours.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Eazy.Tours.Controllers
{
    [Authorize(Roles = Constants.Roles.Administrator)]
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region APICALL

        public IActionResult AllOrders(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole("Administrator") || User.IsInRole("Employee"))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == claims.Value);
            }

            switch (status)
            {
                case "Pending":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == PaymentStatus.StatusPending);
                    break;
                case "Approved":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == PaymentStatus.StatusApproved);
                    break;
                case "InProcess":
                    orderHeaders = orderHeaders.Where(x => x.BookingStatus == BookingStatus.StatusInProcess);
                    break;
                case "Booked":
                    orderHeaders = orderHeaders.Where(x => x.BookingStatus == BookingStatus.StatusBooked);
                    break;
                default:
                    break;
            }
            return Json(new {data = orderHeaders});
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult OrderDetails(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == id, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product")
            };
            return View(orderVM);
        }

        [Authorize(Roles = WebsiteRole.Role_Administrator + "," + WebsiteRole.Role_Employee)]
        [HttpPost]
        public IActionResult OrderDetails(OrderVM vm)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == vm.OrderHeader.Id);
            orderHeader.FirstName = vm.OrderHeader.FirstName;
            orderHeader.LastName = vm.OrderHeader.LastName;
            orderHeader.PhoneNumber = vm.OrderHeader.PhoneNumber;

            //carrier not needed ?????????????????
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Booking updated";
            return RedirectToAction("OrderDetails", "Order", new { id = vm.OrderHeader.Id});

            //Is it booking or order here?
        }

        [Authorize(Roles = WebsiteRole.Role_Administrator + "," + WebsiteRole.Role_Employee)]
        public IActionResult InProcess(OrderVM vm)
        {
            _unitOfWork.OrderHeader.UpdateStatus(vm.OrderHeader.Id, BookingStatus.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Booking status is updated to In Process";
            return RedirectToAction("OrderDetails", "Order", new {id = vm.OrderHeader.Id});
            //Is it booking or order here?
            //Should i put booking id instead of orderheader id?
        }

        [Authorize(Roles = WebsiteRole.Role_Administrator + "," + WebsiteRole.Role_Employee)]
        public IActionResult Booked(OrderVM vm)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == vm.OrderHeader.Id);
            orderHeader.BookingStatus = BookingStatus.StatusBooked;
            orderHeader.DateOfBooking = DateTime.Now;
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Booking status is updated to Booked";
            return RedirectToAction("OrderDetails", "Order", new { id = vm.OrderHeader.Id });
        }

        [Authorize(Roles = WebsiteRole.Role_Administrator + "," + WebsiteRole.Role_Employee)]
        public IActionResult CancelBooking(OrderVM vm)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == vm.OrderHeader.Id);
            if (orderHeader.PaymentStatus == PaymentStatus.StatusApproved)
            {
                var refund = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service = new RefundService();
                Refund Refund = service.Create(refund);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, BookingStatus.StatusCancelled, BookingStatus.StatusRefund);

            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, BookingStatus.StatusCancelled, BookingStatus.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Booking cancelled";
            //return RedirectToAction("OrderDetails", "Order", new { id = vm.OrderHeader.Id });
            return View(vm);
        }

        [Authorize(Roles = WebsiteRole.Role_Administrator + "," + WebsiteRole.Role_Employee)]
        public IActionResult PayNow(OrderVM vm)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == vm.OrderHeader.Id, includeProperties: "ApplicationUser");
            var orderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == vm.OrderHeader.Id, includeProperties: "Product");
            var domain = "https://localhost:7271";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/cart/ordersucess?id={vm.OrderHeader.Id}",
                CancelUrl = domain + $"/cart/Index",
            };

            foreach (var item in orderDetail)
            {
                var lineItemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "EUR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(lineItemsOptions);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.PaymentStatus(vm.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            return RedirectToAction("Index", "Home");
        }
    }
}
