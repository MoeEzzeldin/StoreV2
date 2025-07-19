using Store.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.I_Repos
{
    public interface I_ProductRepo
    {
        // Synchronous operations with IQueryable for LINQ operations
        IQueryable<Product> GetAllProducts();
        IQueryable<Product> GetProductsByType(string type);
        IQueryable<Product> GetProductsByBrand(string brand);
        IQueryable<Product> SearchProducts(string searchTerm);


        // Asynchronous operations
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string type);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        
        // Additional useful operations
        Task<int> GetProductCountAsync();
        Task<decimal> GetMinPriceAsync();
        Task<decimal> GetMaxPriceAsync();
        Task<IEnumerable<string>> GetAllBrandsAsync();
        Task<IEnumerable<string>> GetAllTypesAsync();
        Task<Dictionary<string, IEnumerable<Product>>> GetProductsByPriceRangeAsync();

    }
}
