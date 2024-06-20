using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository.IRepository;
using GFoods.Models;
using GFoods.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.DbInitalializer
{
    public class DbInitalizer : IDbInitalizer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitalizer(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                     _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }


            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "adminGfoods@gfoods.com",
                Email = "adminGfoods@gfoods.com",
                Name = "AdminGfoods",
                PhoneNumber = "1234567890",
                StreetAddress = "1234567890",
                State = "IL",
                PostalCode = "23422",
                City = "Chicago",
                Coin = 5000
            }, "Admin123*").GetAwaiter().GetResult();
            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "adminGfoods@gfoods.com");
            _userManager.AddToRoleAsync(user,SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
        
    }
}
