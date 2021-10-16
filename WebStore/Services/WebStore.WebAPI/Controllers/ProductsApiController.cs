using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]    
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData productData)
        {
            _ProductData = productData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var sections = _ProductData.GetSections();
            return Ok(sections);
        }

        [HttpGet("sections/{id}")]
        public IActionResult GetSection(int id)
        {
            var section = _ProductData.GetSectionById(id);
            return Ok(section);
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _ProductData.GetBrands();
            return Ok(brands);
        }

        [HttpGet("brands/{id}")]
        public IActionResult GetBrand(int id)
        {
            var brand = _ProductData.GetBrandById(id);
            return Ok(brand);
        }

        [HttpPost]
        public IActionResult GetProducts([FromBody] ProductFilter filter = null)
        {
            var products = _ProductData.GetProducts(filter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        IActionResult GetProductById(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();
            return Ok(product);
        }
    }
}
