using GFoods.DataAccess.Data;
using GFoods.DataAccess.Repository;
using GFoods.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using GFoods.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(100);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
 
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services
    .AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IVnPayRepository, VnPayReponsitory>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.MapControllerRoute(
    name: "TrangChu",
    pattern: "trang-chu",
    defaults: new { area = "Customer", controller = "Home", action = "Index" }
);
app.MapControllerRoute(
    name: "Sanpham",
    pattern: "san-pham/",
    defaults: new { area = "Customer", controller = "Products", action = "Index" }
);
app.MapControllerRoute(
    name: "TinTuc",
    pattern: "tin-tuc",
    defaults: new { area = "Customer", controller = "News", action = "Index" }
);
app.MapControllerRoute(
    name: "KhuyenMai",
    pattern: "khuyen-mai",
    defaults: new { area = "Customer", controller = "Posts", action = "Index" }
);
app.MapControllerRoute(
    name: "GioHang",
    pattern: "gio-hang",
    defaults: new { area = "Customer", controller = "Cart", action = "Index" }
);
app.MapControllerRoute(
    name: "ChiTiet",
    pattern: "chi-tiet/{alias}-p{id}/",
    defaults: new { area = "Customer", controller = "Products", action = "Detail" }
);
app.MapControllerRoute(
    name: "Productcategory",
    pattern: "danh-muc-san-pham/{alias}-h{id}/",
    defaults: new { area = "Customer", controller = "Products", action = "ProductCategory1"}
);
app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "MyAreaProducts",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.Run();
