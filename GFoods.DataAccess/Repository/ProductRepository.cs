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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(x=>x.Id == product.Id);
            if(objFromDb != null)
            {
                objFromDb.ProductCode = product.ProductCode;
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.Detail = product.Detail;
                objFromDb.OriginalPrice = product.OriginalPrice;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.PriceSale = product.PriceSale;
                objFromDb.Price = product.Price;
                objFromDb.Quantity = product.Quantity;
            }
            if(product.ImageUrl != null)
            {
                objFromDb.ImageUrl = objFromDb.ImageUrl;
            }
        }
    }
}
