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
        

        public void Update(CategoryProduct categoryProduct)
        {
            _db.CategoryProducts.Update(categoryProduct);
        }
    }
}
