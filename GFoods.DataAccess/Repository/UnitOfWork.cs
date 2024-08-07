﻿using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryProductRepository CategoryProduct {  get; private set; }
        public IProductRepository Product {  get; private set; }
        public IProductImagesRepository ProductImage {  get; private set; }
        public ICompanyRepository Company {  get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            CategoryProduct = new CategoryProductRepository(_db);
            Product = new ProductRepository(_db);
            ProductImage = new ProductImagesRepository(_db);
            Company = new CompanyRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            Category = new CategoryRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();

        }
    }
}
