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
    public class ProductImagesRepository : Repository<ProductImages>, IProductImagesRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductImagesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        

        public void Update(ProductImages productImages)
        {
            _db.ProductImages.Update(productImages);
        }


    }
}
