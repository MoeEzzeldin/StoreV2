using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Services.I_AppService;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly I_ProductService _productService;
        public ProductController(ILogger<ProductController> logger, I_ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // GET: /Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            _logger.LogInformation("Getting all products");
            IEnumerable<ProductDTO> products;
            // Fetch all products asynchronously
            try
            {
                products = await _productService.GetAllProductsAsync();

                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found");
                    return NotFound("No products available");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products");
                return StatusCode(500, "Internal server error");
            }
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            ProductDTO product;
            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}", id);
                return BadRequest("Invalid product ID");
            }
            // Fetch product by ID asynchronously
            try
            {
                product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found", id);
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID: {ProductId}", id);
                return StatusCode(500, "Internal server error");
            }

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found", id);
                return NotFound();
            }

             return Ok(product);
        }

        [HttpGet("/product/cards")]
        public async Task<ActionResult<Dictionary<string, IEnumerable<ProductDTO>>>> GetProductsByPriceRange()
        {
            _logger.LogInformation("Getting products by price ranges");
            Dictionary<string, List<ProductDTO>> productsByPriceRange = await _productService.GetProductsByPriceRangeAsync();
            return Ok(productsByPriceRange);
        }


        //// GET: api/Product/type/electronics
        //[HttpGet("type/{type}")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProductsByType(string type)
        //{
        //    _logger.LogInformation("Getting products by type: {Type}", type);
        //    var products = await _productRepo.GetProductsByTypeAsync(type);
        //    return Ok(products);
        //}



        //// GET: api/Product/brand/apple
        //[HttpGet("brand/{brand}")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProductsByBrand(string brand)
        //{
        //    _logger.LogInformation("Getting products by brand: {Brand}", brand);
        //    var products = await _productRepo.GetProductsByBrandAsync(brand);
        //    return Ok(products);
        //}

        //// GET: api/Product/search?term=laptop
        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string term)
        //{
        //    _logger.LogInformation("Searching products with term: {Term}", term);
        //    var products = await _productRepo.SearchProductsAsync(term);
        //    return Ok(products);
        //}

        //// POST: api/Product
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<Product>> CreateProduct(Product product)
        //{
        //    _logger.LogInformation("Creating new product: {ProductName}", product.Name);
        //    var createdProduct = await _productRepo.AddProductAsync(product);
        //    return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
        //}

        //// PUT: api/Product/5
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateProduct(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        _logger.LogWarning("Product ID mismatch: {ProvidedId} vs {ProductId}", id, product.ProductId);
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        //        await _productRepo.UpdateProductAsync(product);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (await _productRepo.GetProductByIdAsync(id) == null)
        //        {
        //            _logger.LogWarning("Product with ID: {ProductId} not found during update", id);
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/Product/5
        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        //    var result = await _productRepo.DeleteProductAsync(id);

        //    if (!result)
        //    {
        //        _logger.LogWarning("Product with ID: {ProductId} not found during delete", id);
        //        return NotFound();
        //    }

        //    return NoContent();
        //}

        //// GET: api/Product/brands
        //[HttpGet("brands")]
        //public async Task<ActionResult<IEnumerable<string>>> GetAllBrands()
        //{
        //    _logger.LogInformation("Getting all unique brands");
        //    var brands = await _productRepo.GetAllBrandsAsync();
        //    return Ok(brands);
        //}

        //// GET: api/Product/types
        //[HttpGet("types")]
        //public async Task<ActionResult<IEnumerable<string>>> GetAllTypes()
        //{
        //    _logger.LogInformation("Getting all unique product types");
        //    var types = await _productRepo.GetAllTypesAsync();
        //    return Ok(types);
        //}

        //// GET: api/Product/5/reviews
        //[HttpGet("{id}/reviews")]
        //public async Task<ActionResult<IEnumerable<Review>>> GetProductReviews(int id)
        //{
        //    _logger.LogInformation("Getting reviews for product with ID: {ProductId}", id);
        //    var product = await _productRepo.GetProductByIdAsync(id);

        //    if (product == null)
        //    {
        //        _logger.LogWarning("Product with ID: {ProductId} not found when getting reviews", id);
        //        return NotFound();
        //    }

        //    var reviews = await _reviewRepo.GetReviewsByProductIdAsync(id);
        //    return Ok(reviews);
        //}

        //// GET: api/Product/5/rating
        //[HttpGet("{id}/rating")]
        //public async Task<ActionResult<double>> GetProductAverageRating(int id)
        //{
        //    _logger.LogInformation("Getting average rating for product with ID: {ProductId}", id);
        //    var product = await _productRepo.GetProductByIdAsync(id);

        //    if (product == null)
        //    {
        //        _logger.LogWarning("Product with ID: {ProductId} not found when getting rating", id);
        //        return NotFound();
        //    }

        //    var rating = await _reviewRepo.GetAverageRatingForProductAsync(id);
        //    return Ok(rating);
        //}
    }
}
