using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Services.Mapping;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __PageSizeConfig = "CatalogPageSize";

        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration configuration )
        {
            _ProductData = ProductData;
            _Configuration = configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int page = 1, int? pageSize = null)
        {
            var page_size = pageSize
                ?? (int.TryParse(_Configuration[__PageSizeConfig], out var value) ? value: null);
            
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = page,
                PageSize = page_size,
            };
                        
            var (products, total_count) = _ProductData.GetProducts(filter);

            var viewModel = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView(),
                PageViewModel = new()
                {
                    Page = page,
                    PageSize = page_size ?? 0,
                    TotalItems = total_count,
                }
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

        public IActionResult GetProductsView(int? BrandId, int? SectionId, int page = 1, int? pageSize = null)
        {
            var products = GetProducts(BrandId, SectionId, page, pageSize);

            return PartialView("Partial/_Products", products);
        }

        public IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int page, int? pageSize)
        {
            var products = _ProductData.GetProducts(new()
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = page,
                PageSize = pageSize ?? _Configuration.GetValue(__PageSizeConfig, 6),
            });

            return products.Products.OrderBy(p => p.Order).ToView();
        }
    }
}
