using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO
{
    public record ProductsPageDTO(IEnumerable<ProductDTO> Products, int totalCount);
    
    public static class ProductsPageDTOMapper
    {
        public static ProductsPageDTO ToDTO(this ProductsPage page) => new(page.Products.ToDTO(), page.totalCount);

        public static ProductsPage FromDTO(this ProductsPageDTO page) => new(page.Products.FromDTO(), page.totalCount);
    }
}
