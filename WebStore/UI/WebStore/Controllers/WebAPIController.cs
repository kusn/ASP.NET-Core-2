using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebAPIController(IValuesService valuesService)
        {
            _ValuesService = valuesService;
        }

        public IActionResult Index()
        {
            var values = _ValuesService.GetAll();

            return View(values);
        }
    }
}
