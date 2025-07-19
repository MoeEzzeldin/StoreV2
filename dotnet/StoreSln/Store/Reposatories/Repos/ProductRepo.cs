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

namespace Store.Reposatories.Repos
{
    public class ProductRepo : I_ProductRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IDapperContext _dapperContext;

        public ProductRepo(ApplicationDbContext context, IDapperContext dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        #region IQueryable Methods (Entity Framework)

        public IQueryable<Product> GetAllProducts()
        {
            return _context.Products.AsQueryable();
        }

        public IQueryable<Product> GetProductsByType(string type)
        {
            return _context.Products
                .Where(p => p.Type.ToLower() == type.ToLower())
                .AsQueryable();
        }

        public IQueryable<Product> GetProductsByBrand(string brand)
        {
            return _context.Products
                .Where(p => p.Brand.ToLower() == brand.ToLower())
                .AsQueryable();
        }

        public IQueryable<Product> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _context.Products.AsQueryable();

            return _context.Products
                .Where(p => p.Name.Contains(searchTerm) || 
                            p.Brand.Contains(searchTerm) || 
                            p.Type.Contains(searchTerm))
                .AsQueryable();
        }

        #endregion

        #region Async Methods (Mix of EF Core and Dapper)

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Product>("SELECT * FROM product");
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Product>(
                "SELECT * FROM product WHERE product_id = @Id", 
                new { Id = id });
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Product>(
                "SELECT * FROM product WHERE type = @Type", 
                new { Type = type });
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Product>(
                "SELECT * FROM product WHERE brand = @Brand", 
                new { Brand = brand });
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            using var connection = _dapperContext.CreateConnection();
            var term = $"%{searchTerm}%";
            return await connection.QueryAsync<Product>(
                "SELECT * FROM product WHERE name LIKE @Term OR brand LIKE @Term OR type LIKE @Term", 
                new { Term = term });
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            // Using EF Core for this operation to handle identity columns and navigation properties
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            using var connection = _dapperContext.CreateConnection();
            await connection.ExecuteAsync(
                @"UPDATE product SET 
                    name = @Name, 
                    brand = @Brand, 
                    type = @Type, 
                    price = @Price, 
                    picture_url = @PictureUrl 
                  WHERE product_id = @ProductId",
                new { 
                    product.Name,
                    product.Brand,
                    product.Type,
                    product.Price,
                    product.PictureUrl,
                    product.ProductId
                });
            
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "DELETE FROM product WHERE product_id = @Id",
                new { Id = id });
            
            return affectedRows > 0;
        }

        #endregion

        #region Additional Methods

        public async Task<int> GetProductCountAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM product");
        }

        public async Task<decimal> GetMinPriceAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteScalarAsync<decimal?>("SELECT MIN(price) FROM product");
            return result ?? 0;
        }

        public async Task<decimal> GetMaxPriceAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteScalarAsync<decimal?>("SELECT MAX(price) FROM product");
            return result ?? 0;
        }

        public async Task<IEnumerable<string>> GetAllBrandsAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<string>("SELECT DISTINCT brand FROM product ORDER BY brand");
        }

        public async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<string>("SELECT DISTINCT type FROM product ORDER BY type");
        }

        public async Task<Dictionary<string, IEnumerable<Product>>> GetProductsByPriceRangeAsync()
        {
            var result = new Dictionary<string, IEnumerable<Product>>();

            // Filter products under $200
            result["under-200"] = await _context.Products
                .Where(p => p.Price < 200)
                .ToListAsync();

            // Filter products under $500 (but over $200)
            result["under-500"] = await _context.Products
                .Where(p => p.Price >= 200 && p.Price < 500)
                .ToListAsync();

            // Filter products over $500
            result["over-500"] = await _context.Products
                .Where(p => p.Price >= 500)
                .ToListAsync();

            return result;
        }
        #endregion
    }
}
