using Store.Models.Entities;
using Store.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace Store.Services.I_AppService
{
    public interface I_ProductService
    {
        //Asynchronous operations for Get
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        //Task<IEnumerable<ProductDTO>> GetProductsByTypeAsync(string type);
        //Task<IEnumerable<ProductDTO>> GetProductsByBrandAsync(string brand);
        //Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<Dictionary<string, List<ProductDTO>>> GetProductsByPriceRangeAsync();



        //// Asynchronous operations for Edits
        //Task<ProductDTO> GetProductByIdForEditAsync(int id);
        //Task<ProductDTO> AddProductAsync(ProductDTO product);
        //Task<ProductDTO> UpdateProductAsync(ProductDTO product);
        //Task<bool> DeleteProductAsync(int id);

        //// Additional useful operations
        //Task<int> GetProductCountAsync();
        //Task<decimal> GetMinPriceAsync();
        //Task<decimal> GetMaxPriceAsync();
    }
}
