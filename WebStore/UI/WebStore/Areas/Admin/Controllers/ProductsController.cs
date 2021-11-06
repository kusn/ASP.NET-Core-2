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
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }
        
        public IActionResult Index()
        {
            var products = _ProductData.GetProducts();

            return View(products.Products);
        }
                
        public IActionResult Edit(int? id)
        {
            var notParentSections = _ProductData.GetSections().Where(s =>
            s.ParentId != null);
            var brands = _ProductData.GetBrands();
            if (!id.HasValue)
            {
                return View(new ProductViewModel()
                {
                    Sections = new SelectList(notParentSections, "Id", "Name"),
                    Brands = new SelectList(brands, "Id", "Name")
                });
            }
            var product = _ProductData.GetProductById(id.Value);
            if (product == null)
                return NotFound();

            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Section = product.Section.Name,
                SectionId = product.Section.Id,
                Brand = product.Brand?.Name,
                BrandId = product.Brand?.Id,
                Brands = new SelectList(brands, "Id", "Name", product.Brand?.Id),
                Sections = new SelectList(notParentSections, "Id", "Name", product.Section.Id)
            });
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            var notParentSections = _ProductData.GetSections().Where(s =>
            s.ParentId != null);
            var brands = _ProductData.GetBrands();
            if (ModelState.IsValid)
            {
                var productDTO = new ProductDTO()
                {
                    Id = model.Id,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Order = model.Order,
                    Price = model.Price,
                    Brand = model.BrandId.HasValue
                    ? new BrandDTO()
                    {
                        Id = model.BrandId.Value
                    }
                    : null,
                    Section = new SectionDTO()
                    {
                        Id = model.SectionId
                    }
                };
                if (model.Id > 0)
                {
                    _ProductData.UpdateProduct(productDTO);
                }
                else
                {
                    _ProductData.CreateProduct(productDTO);
                }
                return RedirectToAction(nameof(Index));
            }
            model.Brands = new SelectList(brands, "Id", "Name", model.BrandId);
            model.Sections = new SelectList(notParentSections, "Id", "Name", model.SectionId);

            return View(model);
        }

        public IActionResult Delete(int id) => RedirectToAction(nameof(Index));
    }
}
