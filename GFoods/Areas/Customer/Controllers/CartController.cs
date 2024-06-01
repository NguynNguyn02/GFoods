using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Models.ViewModels;
using GFoods.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GFoods.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayRepository _vnPayService;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IVnPayRepository vnPayService)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new(),
               
            };
            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                item.Price = GetPriceBaseOnQuantity(item);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price * item.Count;
            }
            return View(ShoppingCartVM);
        }

        private double GetPriceBaseOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Product.PriceSale != null)
            {

                return (double)shoppingCart.Product.PriceSale;
            }
            else
            {
                return (double)shoppingCart.Product.Price;
            }
        }
        public IActionResult Plus(int cartId)
        {
            var cartofDB = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            cartofDB.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartofDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        
        

        public IActionResult Minus(int cartId)
        {
            var cartofDB = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            if (cartofDB.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartofDB);
            }
            else
            {
                cartofDB.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartofDB);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartofDB = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartofDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: nameof(Product)),
                OrderHeader = new()

            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                item.Price = GetPriceBaseOnQuantity(item);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price * item.Count;
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM, string payment)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: nameof(Product));

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            var orderIdTemporary = ShoppingCartVM.OrderHeader.Id;
            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                item.Price = GetPriceBaseOnQuantity(item);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price * item.Count;
            }
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
                TempData["OrderDetail"] = JsonConvert.SerializeObject(orderDetail);
            }
            if (payment == "Thanh toán bằng VNPAY")
            {
                var requestModel = new VnPaymentRequestModel
                {
                    Amount = ShoppingCartVM.OrderHeader.OrderTotal,
                    CreatedDate = DateTime.Now,
                    Description = $"Thanh toán đơn hàng {orderIdTemporary}",
                    FullName = applicationUser.Name,
                    OrderHeaderId = orderIdTemporary,
                };
                TempData["RequestModel"] = JsonConvert.SerializeObject(requestModel);
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, requestModel));
            }
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //day la tai khoan khach hang binh thuong
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Update(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();


            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult PaymentOnline()
        {
            return View();
        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
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
                return RedirectToAction(nameof(OrderConfirmation), new { id = requestModel.OrderHeaderId });
            }
            return RedirectToAction(nameof(PaymentFail));

        }
    }
}
