using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using Microsoft.AspNetCore.Mvc;

namespace GFoods.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;

        private readonly IUnitOfWork _unitOfWork;
        public MenuController(ILogger<MenuController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuTop()
        {
            List<Category> item = _unitOfWork.Category.GetAll().OrderBy(x=>x.DisplayOrder).ToList();
            return PartialView("_MenuTop", item);
        }
        //public IActionResult MenuProductCategory()
        //{
        //    var items = db.ProductCategories.ToList();

        //    return PartialView("_MenuProductCategory", items);

        //}
        //public IActionResult MenuLeft(int? id)
        //{
        //    if (id != null)
        //    {
        //        ViewBag.CateId = id;
        //    }
        //    var items = db.ProductCategories.ToList();

        //    return PartialView("_MenuLeft", items);

        //}
        //public IActionResult MenuArrivals()
        //{
        //    var items = db.ProductCategories.ToList();

        //    return PartialView("_MenuArrivals", items);

        //}
    }
}
