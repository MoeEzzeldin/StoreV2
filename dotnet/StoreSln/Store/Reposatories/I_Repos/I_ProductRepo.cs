using Store.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.I_Repos
{
    public interface I_ProductRepo
    {
        // Synchronous operations with IQueryable for LINQ operations "Display methods"
        // These methods work for Read-Only operations "AsNoTracking" will not keep addresses of entities in the change tracker
        IQueryable<Product> GetAllProducts();
        IQueryable<Product> GetProductById(int id);
        //IQueryable<Product> GetProductsByType(string type);
        //IQueryable<Product> GetProductsByBrand(string brand);
        //IQueryable<Product> SearchProducts(string searchTerm);
        Task<Dictionary<string, List<Product>>> GetProductsByPriceRangeAsync();



        //// Asynchronous operations for Edits
        //Task<Product> GetProductByIdForEditAsync(int id);
        //Task<Product> AddProductAsync(Product product);
        //Task<Product> UpdateProductAsync(Product product);
        //Task<bool> DeleteProductAsync(int id);
        
        //// Additional useful operations
        //Task<int> GetProductCountAsync();
        //Task<decimal> GetMinPriceAsync();
        //Task<decimal> GetMaxPriceAsync();
    }
}
