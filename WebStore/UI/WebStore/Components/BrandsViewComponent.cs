using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    //[ViewComponent(Name = "BrandsView")]
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        /*public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }*/

        public IViewComponentResult Invoke()
        {
            return View(GetBrands());
        }

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
            }
            );
    }
}
