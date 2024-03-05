using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using Microsoft.AspNetCore.Mvc;

namespace GFoods.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index()
        {
            List<Category> items = _unitOfWork.Category.GetAll().OrderBy(x=>x.DisplayOrder).ToList();
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (model.Name == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name cannot similar DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(model);
                _unitOfWork.Save();
                TempData["Success"] = "Success Create";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? item = _unitOfWork.Category.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(model);
                _unitOfWork.Save();
                TempData["Success"] = "Success Edit";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? item = _unitOfWork.Category.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            Category? item = _unitOfWork.Category.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(item);
            _unitOfWork.Save();
            TempData["Success"] = "Success Delete";
            return RedirectToAction("Index");


        }
    }
}
