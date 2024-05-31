using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                listProduct = listProduct.Where(x=>x.CategoryProductId == id).ToList();
            }
            return View(listProduct);
        }
        public IActionResult Detail(string alias, int id)
        {
            var product = _unitOfWork.Product.Get(x=>x.Id == id,includeProperties: "CategoryProduct,ProductImages");
            return View(product);
        }
        public IActionResult ProductCategory1(string alias, int ?id)
        {
            var product = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct");
            if (id > 0)
            {
                product = product.Where(x => x.CategoryProductId == id).ToList();
            }
            var cate = _unitOfWork.CategoryProduct.Get(x=>x.Id==id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Name;
            }
            ViewBag.CateId = id;
            return View(product);
        }
        public IActionResult Partial_ProductSale()
        {
            var items = _unitOfWork.Product.GetAll(x => x.IsSale && x.IsHome,includeProperties: "CategoryProduct").ToList();
            return PartialView("_Partial_ProductSale",items);
        }

    }
}
