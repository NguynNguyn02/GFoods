using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Models.ViewModels;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<CategoryProduct> items = _unitOfWork.CategoryProduct.GetAll().OrderBy(x => x.DisplayOrder).ToList();
            return View(items);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new CategoryProduct());
            }
            else
            {
                CategoryProduct categoryProduct = _unitOfWork.CategoryProduct.Get(x => x.Id == id);
                return View(categoryProduct);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryProduct categoryProduct, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\categoryproduct");
                    if (!string.IsNullOrEmpty(categoryProduct.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, categoryProduct.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    categoryProduct.ImageUrl = @"\images\categoryproduct\" + fileName;
                }
                if (categoryProduct.Id == 0)
                {
                    categoryProduct.CreatedDate = DateTime.Now;
                    categoryProduct.ModifiedDate = DateTime.Now;
                    categoryProduct.CreatedBy = User.Identity.Name;
                    categoryProduct.ModifiedBy = User.Identity.Name;
                    if (string.IsNullOrEmpty(categoryProduct.SeoTitle))
                    {
                        categoryProduct.SeoTitle = categoryProduct.Name;
                    }
                    if (string.IsNullOrEmpty(categoryProduct.Alias))
                    {
                        categoryProduct.Alias = GFoods.Models.Common.Filter.FilterChar(categoryProduct.Name);
                    }
                    _unitOfWork.CategoryProduct.Add(categoryProduct); ;
                }
                else
                {
                    categoryProduct.ModifiedDate = DateTime.Now;
                    categoryProduct.ModifiedBy = User.Identity.Name;
                    categoryProduct.Alias = GFoods.Models.Common.Filter.FilterChar(categoryProduct.Name);
                    _unitOfWork.CategoryProduct.Update(categoryProduct);

                }
                _unitOfWork.Save();
                TempData["Success"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryProduct);
            }


        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<CategoryProduct> categoryProducts = _unitOfWork.CategoryProduct.GetAll().ToList();
            return Json(new { data = categoryProducts });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var categoryProductToBeDeleted = _unitOfWork.CategoryProduct.Get(x => x.Id == id);
            if (categoryProductToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.CategoryProduct.Remove(categoryProductToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion
    }
}
