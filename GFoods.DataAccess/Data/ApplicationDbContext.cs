using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFoods.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GFoods.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CategoryProduct>().HasData(
                new CategoryProduct { Id=1,Name="Tây Bắc",DisplayOrder=1},
                new CategoryProduct { Id=2,Name="Nghệ An",DisplayOrder=2},
                new CategoryProduct { Id=3,Name="Hà Nội",DisplayOrder=3}
                );
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "N1", StreetAddress = "1", City = "City1", PostalCode = "PostalCode1", State = "IL1", PhoneNumber = "01234567891" },
                new Company { Id = 2, Name = "N2", StreetAddress = "2", City = "City2", PostalCode = "PostalCode2", State = "IL2", PhoneNumber = "01234567892" },
                new Company { Id = 3, Name = "N3", StreetAddress = "3", City = "City3", PostalCode = "PostalCode3", State = "IL3", PhoneNumber = "01234567893" }
           );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Nguyen1",
                    ProductCode = "SP1",
                    Description = "Description1",
                    Detail = "DetailsNguyen",
                    Quantity = 5,
                    ImageUrl = "",
                    OriginalPrice = 1000,
                    Price = 1200,
                    PriceSale = 1100,
                    CategoryProductId = 1,
                },
                new Product
                {
                    Id = 2,
                    Title = "Nguyen2",
                    ProductCode = "SP02",
                    Description = "Description2",
                    Detail = "DetailNguyen2",
                    Quantity = 10,
                    ImageUrl = "",
                    OriginalPrice = 2000,
                    Price = 2200,
                    PriceSale = 2100,
                    CategoryProductId = 2

                },
                new Product
                {
                    Id = 3,
                    Title = "Nguyen3",
                    ProductCode = "SP03",
                    Description = "Description3",
                    Detail = "DetailNguyen",
                    Quantity = 15,
                    ImageUrl = "",
                    OriginalPrice = 3000,
                    Price = 3200,
                    PriceSale = 3100,
                    CategoryProductId = 3
                }
                );
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "N1", DisplayOrder = 1 },
                new Category { Id = 2, Name = "N2", DisplayOrder = 2 },
                new Category { Id = 3, Name = "N3", DisplayOrder = 3 }
           );
        }
    }
}
