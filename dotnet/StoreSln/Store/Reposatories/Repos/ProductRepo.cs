using Microsoft.EntityFrameworkCore;
using Dapper;
using Store.Data;
using Store.Reposatories.I_Repos;
using Store.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models.Entities;
using Store.Exceptions;


namespace Store.Reposatories.Repos
{
    public class ProductRepo : I_ProductRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IDapperContext _dapperContext;
        private readonly ILogger<ProductRepo> _logger;

        public ProductRepo(ApplicationDbContext context, IDapperContext dapperContext, ILogger<ProductRepo> logger)
        {
            _context = context;
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
            _logger = logger;
        }

        #region IQueryable Methods (Entity Framework)

        public IQueryable<Product> GetAllProducts()
        {
            try
            {
                return _context.Products.AsQueryable().AsNoTracking();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to access Products DbSet");
                throw new DataAccessException("Unable to retrieve products", ex);
            }
        }

        public IQueryable<Product> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("GetProductById called with invalid id: {Id}", id);
                    return Enumerable.Empty<Product>().AsQueryable();
                }

                return _context.Products
                    .Where(p => p.ProductId == id)
                    .AsQueryable().AsNoTracking();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to query product with id: {Id}", id);
                throw new DataAccessException($"Unable to retrieve product with id: {id}", ex);
            }
        }

        //public IQueryable<Product> GetProductsByType(string type)
        //{
        //    return _context.Products
        //        .Where(p => p.Type.ToLower() == type.ToLower())
        //        .AsQueryable();
        //}

        //public IQueryable<Product> GetProductsByBrand(string brand)
        //{
        //    return _context.Products
        //        .Where(p => p.Brand.ToLower() == brand.ToLower())
        //        .AsQueryable();
        //}

        //public IQueryable<Product> SearchProducts(string searchTerm)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //        return _context.Products.AsQueryable();

        //    return _context.Products
        //        .Where(p => p.Name.Contains(searchTerm) || 
        //                    p.Brand.Contains(searchTerm) || 
        //                    p.Type.Contains(searchTerm))
        //        .AsQueryable();
        //}
        #endregion

        #region Async Methods (Mix of EF Core and Dapper)
        public async Task<Dictionary<string, List<Product>>> GetProductsByPriceRangeAsync()
        {
            try
            {
                List<Product> under200 = await _context.Products.Where(p => p.Price < 200).Include(p => p.Reviews).ToListAsync();
                List<Product> under500 = await _context.Products.Where(p => p.Price >= 200 && p.Price < 500).Include(p => p.Reviews).ToListAsync();
                List<Product> over500 = await _context.Products.Where(p => p.Price >= 500).Include(p => p.Reviews).ToListAsync();

                //await Task.WhenAll(under200, under500, over500);

                Dictionary<string, List<Product>> result = new()
                {
                    ["under-200"] = under200,
                    ["under-500"] = under500,
                    ["over-500"] = over500
                };

                // Check if all lists are empty (no products found)
                if (!result.Values.Any(list => list.Any()))
                {
                    _logger.LogInformation("No products found in any price range");
                    return new Dictionary<string, List<Product>>(); // Return empty dictionary for no results
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching products by price range");
                throw;
            }
        }
        //public async Task<Product> GetProductByIdAsync(int id)
        //{
        //    using var connection = _dapperContext.CreateConnection();
        //    return await connection.QuerySingleOrDefaultAsync<Product>(
        //        "SELECT * FROM product WHERE product_id = @Id", 
        //        new { Id = id });
        //}


        //public async Task<Product> AddProductAsync(Product product)
        //{
        //    // Using EF Core for this operation to handle identity columns and navigation properties
        //    await _context.Products.AddAsync(product);
        //    await _context.SaveChangesAsync();
        //    return product;
        //}

        //public async Task<Product> UpdateProductAsync(Product product)
        //{
        //    using var connection = _dapperContext.CreateConnection();
        //    await connection.ExecuteAsync(
        //        @"UPDATE product SET 
        //            name = @Name, 
        //            brand = @Brand, 
        //            type = @Type, 
        //            price = @Price, 
        //            picture_url = @PictureUrl 
        //          WHERE product_id = @ProductId",
        //        new { 
        //            product.Name,
        //            product.Brand,
        //            product.Type,
        //            product.Price,
        //            product.PictureUrl,
        //            product.ProductId
        //        });

        //    return product;
        //}

        //public async Task<bool> DeleteProductAsync(int id)
        //{
        //    using var connection = _dapperContext.CreateConnection();
        //    var affectedRows = await connection.ExecuteAsync(
        //        "DELETE FROM product WHERE product_id = @Id",
        //        new { Id = id });

        //    return affectedRows > 0;
        //}

        #endregion

        #region Additional Methods

        public async Task<int> GetProductCountAsync()
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                // Add timeout for long-running queries
                var count = await connection.QuerySingleOrDefaultAsync<int?>(
                    "SELECT COUNT(*) FROM product",
                    commandTimeout: 30);

                if (!count.HasValue)
                {
                    _logger.LogWarning("Product count query returned null");
                    return 0;
                }

                _logger.LogInformation("Retrieved product count: {Count}", count);
                return count.Value;
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogError(ioEx, "Connection error while getting product count");
                throw new DataAccessException("Unable to connect to database", ioEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting product count");
                throw;
            }
        }

        //public async Task<decimal> GetMinPriceAsync()
        //{
        //    using var connection = _dapperContext.CreateConnection();
        //    var result = await connection.ExecuteScalarAsync<decimal?>("SELECT MIN(price) FROM product");
        //    return result ?? 0;
        //}

        //public async Task<decimal> GetMaxPriceAsync()
        //{
        //    using var connection = _dapperContext.CreateConnection();
        //    var result = await connection.ExecuteScalarAsync<decimal?>("SELECT MAX(price) FROM product");
        //    return result ?? 0;
        //}
        #endregion
    }
}
