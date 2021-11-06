using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class HomeController : Controller
    {
        private readonly IProductData _ProductData;

        public HomeController(IProductData productData)
        {
            _ProductData = productData;
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
