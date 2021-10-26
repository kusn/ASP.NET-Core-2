using Microsoft.AspNetCore.Mvc;
using System;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()                     // http://localhost:5000/Home/Index
        {
            return View();
        }

        public IActionResult Exception(string message) => throw new InvalidOperationException(message ?? "Ошибка в контроллере!");

        public IActionResult Status(string Code)
        {
            if (Code is null)
                throw new ArgumentNullException(nameof(Code));

            if (Code == "404")
                return Redirect("/NotFound/Index");     //Перенаправление на http://localhost:5000/NotFound/Index
            return Content($"Статусный код: {Code}");
        }
    }
}
