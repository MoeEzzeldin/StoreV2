using Store.Services.I_AppService;
using Store.Reposatories.I_Repos;
using Store.Models.Entities;
using Store.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Store.Services.AppService
{
    public class ProductService : I_ProductService
    {
        private readonly I_ProductRepo _productRepo;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(I_ProductRepo productRepo, ILogger<ProductService> logger, IMapper mapper)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Async Methods
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                List<Product> products = await _productRepo.GetAllProducts().ToListAsync();

                if (products == null || !products.Any())
                {
                    _logger.LogInformation("No products found.");
                    return new List<ProductDTO>();
                }
                return _mapper.Map<List<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                throw;
            }
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {Id}", id);
                throw new ArgumentException("Product must exist with a valid ID", nameof(id));
            }

            try
            {
                Product? product = await _productRepo.GetProductById(id).AsNoTracking().FirstOrDefaultAsync();
                if (product == null)
                {
                    _logger.LogInformation("Product not found for ID: {Id}", id);
                    // Optionally throw or return null
                    return null;
                }
                _logger.LogInformation("Product retrieved successfully for ID: {Id}", id);
                // Map the product entity to ProductDTO
                ProductDTO productDto = _mapper.Map<ProductDTO>(product);
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID: {Id}", id);
                throw;
            }

        }
    }
    #endregion
}