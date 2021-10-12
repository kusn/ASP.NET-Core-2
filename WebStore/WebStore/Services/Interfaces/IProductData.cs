using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        public Section GetSectionById(int id);

        IEnumerable<Brand> GetBrands();

        public Brand GetBrandById(int id);

        IEnumerable<Product> GetProducts( ProductFilter filter = null);

        Product GetProductById(int id);
    }
}
