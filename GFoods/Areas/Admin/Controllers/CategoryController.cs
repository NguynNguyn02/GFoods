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
        public IActionResult Upsert(int? id)
        {


            if (id == null || id == 0)
            {
                return View(new Category());
            }
            else
            {
                Category category = _unitOfWork.Category.Get(x => x.Id == id);
                return View(category);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    category.CreatedDate = DateTime.Now;
                    category.CreatedBy = User.Identity.Name;
                    category.ModifiedBy = User.Identity.Name;
                    category.ModifiedDate = DateTime.Now;
                    category.Alias = GFoods.Models.Common.Filter.FilterChar(category.Name);
                    _unitOfWork.Category.Add(category);
                }
                else
                {
                    category.ModifiedBy = User.Identity.Name;
                    category.ModifiedDate = DateTime.Now;
                    category.Alias = GFoods.Models.Common.Filter.FilterChar(category.Name);
                    _unitOfWork.Category.Update(category);

                }
                _unitOfWork.Save();
                TempData["Success"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return Json(new { data = categories });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var categoryToBeDeleted = _unitOfWork.Category.Get(x => x.Id == id);
            if (categoryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Category.Remove(categoryToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion
    }
}
