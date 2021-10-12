using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()                     // http://localhost:5000/Home/Index
        {
            return View();
        }

        public IActionResult Status(string Code)
        {
            if (Code == "404")
                return Redirect("/NotFound/Index");     //Перенаправление на http://localhost:5000/NotFound/Index
            return Content($"Статусный код: {Code}");
        }
    }
}
