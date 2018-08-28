using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Databas;
using Databas.Models;

namespace Kzta.Domain.Products
{
    /// <summary>
    /// API для изделий
    /// </summary>
    public class ProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создание изделия
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Guid> Create(ProductInfo model)
        {
            var result = Mapper.Map<Product>(model);
            result.ProductGuid = Guid.NewGuid();
            _context.Products.Add(result);
            await _context.SaveChangesAsync();
            return result.ProductGuid;
        }

        /// <summary>
        /// Удаляет продукт
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        public async Task Delete(Guid productGuid)
        {
            var result = await _context.Products.SingleOrDefaultAsync(x => x.ProductGuid == productGuid);
            if (result == null)
                return;
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Изменяет продукт
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Edit(Guid productGuid, ProductInfo model)
        {
            var result = await _context.Products.SingleOrDefaultAsync(x => x.ProductGuid == productGuid);
            if (result == null)
                return;
            Mapper.Map(model, result);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Устанавливает картинку
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="imageGuid"></param>
        /// <returns></returns>
        public async Task SetImage(Guid productGuid, Guid imageGuid)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductGuid == productGuid);
            var image = await _context.Files.SingleOrDefaultAsync(x => x.FileGuid == imageGuid);
            if (product == null || image==null)
                return;
            product.FileGuid = image.FileGuid;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Возвращает все продукты
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> Get()
        {
            return await _context.Products
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddDetailToProduct(DetailToProductInfo model)
        {
            if (model == null)
                return;
            var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductGuid == model.ProductGuid);
            var detail = await _context.Details.SingleOrDefaultAsync(x => x.DetailtGuid == model.DetailtGuid);
            if (product == null || detail == null)
                return;
            if (await _context.DetailsInProducts.AnyAsync(x => x.ProductGuid == model.ProductGuid
                                                               && x.DetailGuid == model.DetailtGuid))
            {
                var detailInProduct = await _context.DetailsInProducts.SingleAsync(x =>
                    x.ProductGuid == model.ProductGuid
                    && x.DetailGuid == model.DetailtGuid);
                detailInProduct.Count += model.Count;
                await _context.SaveChangesAsync();
                return;
            }
                var result = Mapper.Map<DetailInProduct>(model);
            _context.DetailsInProducts.Add(result);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Возвращает детали с кол-во для данного продукта
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        public async Task<List<DetailWithCount>> GetDetailWithProduct(Guid productGuid)
        {
            return await _context.DetailsInProducts
                .Where(x => x.ProductGuid == productGuid)
                .AsNoTracking()
                .ProjectTo<DetailWithCount>()
                .ToListAsync();

        }
    }
}
