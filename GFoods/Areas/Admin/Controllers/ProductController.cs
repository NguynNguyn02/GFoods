using GFoods.DataAccess.Repository;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Models.ViewModels;
using GFoods.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GFoods.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> listProduct = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct").ToList();
            return View(listProduct);
        }
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryProduct.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile>? files)
        {

            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    productVM.Product.CreatedDate = DateTime.Now;
                    productVM.Product.ModifiedDate = DateTime.Now;
                    productVM.Product.CreatedBy = User.Identity.Name;
                    productVM.Product.ModifiedBy = User.Identity.Name;
                    if (string.IsNullOrEmpty(productVM.Product.SeoTitle))
                    {
                        productVM.Product.SeoTitle = productVM.Product.Title;
                    }
                    if (string.IsNullOrEmpty(productVM.Product.Alias))
                    {
                        productVM.Product.Alias = GFoods.Models.Common.Filter.FilterChar(productVM.Product.Title);
                    }
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    productVM.Product.ModifiedDate = DateTime.Now;
                    productVM.Product.ModifiedBy = User.Identity.Name;
                    productVM.Product.Alias = GFoods.Models.Common.Filter.FilterChar(productVM.Product.Title);
                    _unitOfWork.Product.Update(productVM.Product);

                }
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);
                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ProductImages productImages = new ProductImages()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,

                        };
                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImages>();

                        productVM.Product.ProductImages.Add(productImages);
                    }
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }



                _unitOfWork.Save();
                TempData["Success"] = "Thành công";

                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryProduct.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productVM);
            }
        }
        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            var productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                        Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Xóa thành công!";
            }
            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "CategoryProduct").ToList();
            return Json(new { data = objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(x => x.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi khi xóa" });
            }
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
            if (Directory.Exists(finalPath)) {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion



    }
}
