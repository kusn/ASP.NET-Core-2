using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddresses.Products)
        {

        }

        public Brand GetBrandById(int id)
        {
            var brand = Get<BrandDTO>($"{Address}/brands/{id}");
            return brand.FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
            return brands.FromDTO();
        }

        public Product GetProductById(int id)
        {
            var product = Get<ProductDTO>($"{Address}/{id}");
            return product.FromDTO();
        }

        public ProductsPage GetProducts(ProductFilter filter = null)
        {
            var response = Post(Address, filter ?? new());
            var products_dtos = response.Content.ReadFromJsonAsync<ProductsPageDTO>().Result;
            return products_dtos.FromDTO();
        }

        public Section GetSectionById(int id)
        {
            var section = Get<SectionDTO>($"{Address}/sections/{id}");
            return section.FromDTO();
        }

        public IEnumerable<Section> GetSections()
        {
            var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
            return sections.FromDTO();
        }

        public SaveResult CreateProduct(ProductDTO productDTO)
        {
            var url = $"{Address}/create";
            var response = Post(url, productDTO);
            var result = response.Content.ReadFromJsonAsync<SaveResult>().Result;
            return result;

        }

        public SaveResult UpdateProduct(ProductDTO productDTO)
        {
            var url = $"{Address}";
            var response = Put(url, productDTO);
            var result = response.Content.ReadFromJsonAsync<SaveResult>().Result;
            return result;

        }

        public SaveResult DeleteProduct(int productId)
        {
            var url = $"{Address}/{productId}";
            var response = DeleteAsync(url).Result;
            var result = response.Content.ReadFromJsonAsync<SaveResult>().Result;
            return result;
        }
    }
}
