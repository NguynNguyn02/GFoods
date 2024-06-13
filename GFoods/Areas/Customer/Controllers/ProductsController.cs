using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GFoods.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? id)
        {
            List<Product> listProduct = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct,ProductImages").ToList();
            if (id != null)
            {
                listProduct = listProduct.Where(x => x.CategoryProductId == id).ToList();
            }
            return View(listProduct);
        }
        public IActionResult Detail(string alias, int id)
        {
            var product = _unitOfWork.Product.Get(x => x.Id == id, includeProperties: "CategoryProduct,ProductImages");
            return View(product);
        }
        public IActionResult ProductCategory1(string alias, int? id)
        {
            var product = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct");
            if (id > 0)
            {
                product = product.Where(x => x.CategoryProductId == id).ToList();
            }
            var cate = _unitOfWork.CategoryProduct.Get(x => x.Id == id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Name;
            }
            ViewBag.CateId = id;
            return View(product);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int ProductId, int Count)
        {
            var code = new
            {
                success = false,
                message = ""
            };
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCart shoppingCart = new ShoppingCart
            {
                ApplicationUserId = userId,
                ProductId = ProductId,
                Count = Count
            };
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId &&
                u.ProductId == ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            _unitOfWork.Save();
            code = new { success = true, message = "Thêm vào giỏ hàng thành công" };
            return Json(code);
        }
        public IActionResult Partial_ProductSale()
        {
            var items = _unitOfWork.Product.GetAll(x => x.IsSale && x.IsHome, includeProperties: "CategoryProduct,ProductImages").ToList();
            return PartialView("_Partial_ProductSale", items);
        }
        public IActionResult Partial_ItemsByCateId()
        {
            var items = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct,ProductImages").ToList();
            return PartialView("_Partial_ItemsByCateId", items);
        }

    }
}
