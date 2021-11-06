using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с товарами
    /// </summary>
    public interface IProductData
    {
        /// <summary>
        /// Список секций
        /// </summary>
        /// <returns></returns>
        IEnumerable<Section> GetSections();

        /// <summary>
        /// Секция по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public Section GetSectionById(int id);

        /// <summary>
        /// Список брендов
        /// </summary>
        /// <returns></returns>
        IEnumerable<Brand> GetBrands();

        /// <summary>
        /// Бренд по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public Brand GetBrandById(int id);

        /// <summary>
        /// Список товаров с постраничным разбиением
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        ProductsPage GetProducts( ProductFilter filter = null);

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашел, иначе null</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        SaveResult CreateProduct(ProductDTO product);

        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        SaveResult UpdateProduct(ProductDTO product);

        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        SaveResult DeleteProduct(int productId);

    }
}
