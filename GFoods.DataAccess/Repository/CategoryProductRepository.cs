using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.Repository
{
    public class CategoryProductRepository : Repository<CategoryProduct>, ICategoryProductRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(CategoryProduct product)
        {
            var objFromDb = _db.CategoryProducts.FirstOrDefault(x => x.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = product.Name;
                objFromDb.DisplayOrder = product.DisplayOrder;
                objFromDb.SeoDescription = product.SeoDescription;
                objFromDb.SeoKeywords = product.SeoKeywords;
                objFromDb.SeoTitle = product.SeoTitle;
            }
            if (product.ImageUrl != null)
            {
                objFromDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}

