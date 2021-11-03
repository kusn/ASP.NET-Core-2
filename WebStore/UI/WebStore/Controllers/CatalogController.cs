using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Services.Mapping;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration configuration )
        {
            _ProductData = ProductData;
            _Configuration = configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int page = 1, int? pageSize = 0)
        {
            var page_size = pageSize
                ?? (int.TryParse(_Configuration["CatalogPageSize"], out var value) ? value: null);
            
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = page,
                PageSize = pageSize,
            };
                        
            var (products, total_count) = _ProductData.GetProducts(filter);

            var viewModel = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView()
            };
            
            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }
    }
}
