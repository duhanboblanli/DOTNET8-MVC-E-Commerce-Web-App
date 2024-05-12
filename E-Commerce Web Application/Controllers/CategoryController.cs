using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Web_Application.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
