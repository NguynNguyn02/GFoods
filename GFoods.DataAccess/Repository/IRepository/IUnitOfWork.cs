using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryProductRepository CategoryProduct { get; }
        IProductRepository Product { get; }
        IProductImagesRepository ProductImage { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        ICategoryRepository Category { get; }
        void Save();

    }
}
