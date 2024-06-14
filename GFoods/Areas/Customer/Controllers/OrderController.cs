using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Models.ViewModels;
using GFoods.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GFoods.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayRepository _vnPayService;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IVnPayRepository vnPayService)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(x => x.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        public IActionResult Details_Pay_Now()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeader.Id == OrderVM.OrderHeader.Id, includeProperties: "Product");
            var requestModel = new VnPaymentRequestModel
            {
                Amount = OrderVM.OrderHeader.OrderTotal,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán đơn hàng {OrderVM.OrderHeader.Id}",
                FullName = OrderVM.OrderHeader.Name,
                OrderHeaderId = OrderVM.OrderHeader.Id,
            };
            TempData["RequestModel"] = JsonConvert.SerializeObject(requestModel);
            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, requestModel));
            return RedirectToAction(nameof(OrderConfirmation), new { id = OrderVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int id)
        {
            HttpContext.Session.Clear();
            return View(id);
        }
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExcute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Error"] = $" Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: {response.VnPayResponseCode}";
                var requestModelJson = TempData["RequestModel"].ToString();
                var requestModel = JsonConvert.DeserializeObject<VnPaymentRequestModel>(requestModelJson);
                OrderHeader header = _unitOfWork.OrderHeader.Get(u => u.Id == requestModel.OrderHeaderId, includeProperties: "ApplicationUser");
                var orderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == requestModel.OrderHeaderId, includeProperties: "Product");
                _unitOfWork.OrderHeader.Remove(header);
                _unitOfWork.OrderDetail.RemoveRange(orderDetail);
                _unitOfWork.Save();
                return RedirectToAction(nameof(PaymentFail));
            }
            if (TempData["RequestModel"] != null || TempData["OrderDetail"] != null)
            {
                var requestModelJson = TempData["RequestModel"].ToString();
                var requestModel = JsonConvert.DeserializeObject<VnPaymentRequestModel>(requestModelJson);
                var orderDetailJson = TempData["OrderDetail"].ToString();
                var orderDetail = JsonConvert.DeserializeObject<OrderDetail>(orderDetailJson);
                ViewData["RequestModel"] = requestModel;
                OrderHeader header = _unitOfWork.OrderHeader.Get(u => u.Id == requestModel.OrderHeaderId, includeProperties: "ApplicationUser");
                header.OrderStatus = SD.StatusApproved;
                header.PaymentStatus = SD.PaymentStatusApproved;
                _unitOfWork.OrderHeader.Update(header);
                _unitOfWork.Save();
                TempData["Success"] = "Thanh toán VNPAY thành công!";
                return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
            }
            return RedirectToAction(nameof(PaymentFail));

        }
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeader;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            objOrderHeader = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            switch (status)
            {
                case "pending":
                    objOrderHeader = objOrderHeader.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeader = objOrderHeader.Where(x => x.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeader = objOrderHeader.Where(x => x.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeader = objOrderHeader.Where(x => x.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderHeader });
        }
        #endregion

    }
}
