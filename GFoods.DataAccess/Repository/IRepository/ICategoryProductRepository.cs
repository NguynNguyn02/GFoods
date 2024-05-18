using GFoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.Repository.IRepository
{
    public interface ICategoryProductRepository : IRepository<CategoryProduct> 
    {
        void Update(CategoryProduct categoryProduct);
    }
}
