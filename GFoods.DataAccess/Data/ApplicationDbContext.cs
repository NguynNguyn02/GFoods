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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action1", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Action2", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Action3", DisplayOrder = 3 }
           );
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Action1", StreetAddress = "1", City = "City1", PostalCode = "PostalCode1", State = "IL1", PhoneNumber = "01234567891" },
                new Company { Id = 2, Name = "Action2", StreetAddress = "2", City = "City2", PostalCode = "PostalCode2", State = "IL2", PhoneNumber = "01234567892" },
                new Company { Id = 3, Name = "Action3", StreetAddress = "3", City = "City3", PostalCode = "PostalCode3", State = "IL3", PhoneNumber = "01234567893" }
           );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Title1",
                    ProductCode = "SP01",
                    Description = "Description1",
                    Detail = "Detail1",
                    Quantity = 5,
                    ImageUrl = "",
                    OriginalPrice = 100,
                    Price = 120,
                    PriceSale = 110,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 2,
                    Title = "Title2",
                    ProductCode = "SP02",
                    Description = "Description2",
                    Detail = "Detail2",
                    Quantity = 10,
                    ImageUrl = "",
                    OriginalPrice = 200,
                    Price = 220,
                    PriceSale = 210,
                    CategoryId = 2

                },
                new Product
                {
                    Id = 3,
                    Title = "Title3",
                    ProductCode = "SP03",
                    Description = "Description3",
                    Detail = "Detail3",
                    Quantity = 15,
                    ImageUrl = "",
                    OriginalPrice = 300,
                    Price = 320,
                    PriceSale = 310,
                    CategoryId = 3

                }
                );
        }
    }
}
