using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFoods.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index()
        {
            List<CategoryProduct> items = _unitOfWork.CategoryProduct.GetAll().OrderBy(x=>x.DisplayOrder).ToList();
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryProduct model)
        {
            if (model.Name == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name cannot similar DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryProduct.Add(model);
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
            CategoryProduct? item = _unitOfWork.CategoryProduct.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryProduct.Update(model);
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
            CategoryProduct? item = _unitOfWork.CategoryProduct.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            CategoryProduct? item = _unitOfWork.CategoryProduct.Get(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryProduct.Remove(item);
            _unitOfWork.Save();
            TempData["Success"] = "Success Delete";
            return RedirectToAction("Index");


        }
    }
}
