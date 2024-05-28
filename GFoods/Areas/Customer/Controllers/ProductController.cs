using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using Microsoft.AspNetCore.Mvc;

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
            List<Product> listProduct = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct").ToList();
            if (id != null)
            {
                listProduct = listProduct.Where(x=>x.CategoryProductId == id).ToList();
            }
            return View(listProduct);
        }
        public IActionResult Index1()
        {
            return View();
        }
    }
}
