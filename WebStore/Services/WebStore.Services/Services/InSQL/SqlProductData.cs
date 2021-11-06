using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands;
        }

        public Brand GetBrandById(int id) => _db.Brands.SingleOrDefault(b => b.Id == id);        

        public ProductsPage GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .Where(c => !c.IsDelete)
                .AsQueryable();

            if (Filter?.Ids?.Length > 0)
            {
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            }
            else
            {
                if (Filter?.SectionId is { } section_id)
                    query = query.Where(p => p.SectionId == section_id);

                if (Filter?.BrandId is { } brand_id)
                    query = query.Where(p => p.SectionId == brand_id);
            }

            var total_count = query.Count();

            if (Filter is { PageSize: > 0 and var page_size, Page: > 0 and var page_number })
                query = query
                    .OrderBy(v => v.Order)
                    .Skip((page_number - 1) * page_size)
                    .Take(page_size);

            return new(query.AsEnumerable(), total_count);
        }

        public Product GetProductById(int id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .SingleOrDefault(p => p.Id == id);

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }

        public Section GetSectionById(int id) => _db.Sections.SingleOrDefault(s => s.Id == id);

        public SaveResult CreateProduct(ProductDTO productDTO)
        {
            try
            {
                var product = new Product()
                {
                    BrandId = productDTO.Brand.Id,
                    SectionId = productDTO.Section.Id,
                    Name = productDTO.Name,
                    ImageUrl = productDTO.ImageUrl,
                    Order = productDTO.Order,
                    Price = productDTO.Price
                };
                _db.Products.Add(product);
                _db.SaveChanges();
                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                return new SaveResult
            
            {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
            };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

        public SaveResult UpdateProduct(ProductDTO productDTO)
        {
            var product = _db.Products.FirstOrDefault();
            if (product == null)
            {
                return new SaveResult()
                {
                    IsSuccess = false,
                    Errors = new List<string>() { "Entity not exist" }
                };
            }
            product.BrandId = productDTO.Brand.Id;
            product.SectionId = productDTO.Section.Id;
            product.ImageUrl = productDTO.ImageUrl;
            product.Order = productDTO.Order;
            product.Price = productDTO.Price;
            product.Name = productDTO.Name;
            try
            {
                _db.SaveChanges();
                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

        public SaveResult DeleteProduct(int productId)
        {
            var product = _db.Products.FirstOrDefault();
            if (product == null)
            {
                return new SaveResult()
                {
                    IsSuccess = false,
                    Errors = new List<string>() { "Entity not exist" }
                };
            }
            try
            {
                //_db.Remove(product);
                product.IsDelete = true;
                _db.SaveChanges();
                return new SaveResult()
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }
    }
}
