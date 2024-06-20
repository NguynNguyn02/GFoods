using Microsoft.AspNetCore.Mvc;

namespace GFoods.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
