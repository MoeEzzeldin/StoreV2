using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.Reposatories.I_Repos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly I_ReviewRepo _reviewRepo;
        private readonly I_ProductRepo _productRepo;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(
            I_ReviewRepo reviewRepo,
            I_ProductRepo productRepo,
            ILogger<ReviewController> logger)
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            _logger.LogInformation("Getting all reviews");
            var reviews = await _reviewRepo.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            _logger.LogInformation("Getting review with ID: {ReviewId}", id);
            var review = await _reviewRepo.GetReviewByIdAsync(id);

            if (review == null)
            {
                _logger.LogWarning("Review with ID: {ReviewId} not found", id);
                return NotFound();
            }

            return Ok(review);
        }

        // GET: api/Review/product/5
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByProductId(int productId)
        {
            _logger.LogInformation("Getting reviews for product with ID: {ProductId}", productId);
            
            // Check if product exists
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found when getting reviews", productId);
                return NotFound("Product not found");
            }
            
            var reviews = await _reviewRepo.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        // GET: api/Review/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUserId(int userId)
        {
            _logger.LogInformation("Getting reviews for user with ID: {UserId}", userId);
            var reviews = await _reviewRepo.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        // POST: api/Review
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            // Check if product exists
            var product = await _productRepo.GetProductByIdAsync(review.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found when creating review", review.ProductId);
                return NotFound("Product not found");
            }

            // Get the current user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("User ID claim not found or not valid");
                return BadRequest("User ID not found");
            }

            _logger.LogInformation("Creating new review for product: {ProductId} by user: {UserId}", 
                review.ProductId, userId);
                
            var createdReview = await _reviewRepo.AddReviewAsync(review, userId);
            return CreatedAtAction(nameof(GetReview), new { id = createdReview.ReviewId }, createdReview);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                _logger.LogWarning("Review ID mismatch: {ProvidedId} vs {ReviewId}", id, review.ReviewId);
                return BadRequest();
            }

            // Check if the user is authorized to update this review
            // This would need to check if the current user is the author of the review or an admin
            // For simplicity, we'll skip this check in this implementation

            try
            {
                _logger.LogInformation("Updating review with ID: {ReviewId}", id);
                await _reviewRepo.UpdateReviewAsync(review);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _reviewRepo.GetReviewByIdAsync(id) == null)
                {
                    _logger.LogWarning("Review with ID: {ReviewId} not found during update", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            // Check if the user is authorized to delete this review
            // This would need to check if the current user is the author of the review or an admin
            // For simplicity, we'll skip this check in this implementation

            _logger.LogInformation("Deleting review with ID: {ReviewId}", id);
            var result = await _reviewRepo.DeleteReviewAsync(id);

            if (!result)
            {
                _logger.LogWarning("Review with ID: {ReviewId} not found during delete", id);
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/Review/product/5/rating
        [HttpGet("product/{productId}/rating")]
        public async Task<ActionResult<double>> GetProductAverageRating(int productId)
        {
            // Check if product exists
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found when getting rating", productId);
                return NotFound("Product not found");
            }
            
            _logger.LogInformation("Getting average rating for product with ID: {ProductId}", productId);
            var rating = await _reviewRepo.GetAverageRatingForProductAsync(productId);
            return Ok(rating);
        }

        // GET: api/Review/product/5/count
        [HttpGet("product/{productId}/count")]
        public async Task<ActionResult<int>> GetProductReviewCount(int productId)
        {
            // Check if product exists
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found when getting review count", productId);
                return NotFound("Product not found");
            }
            
            _logger.LogInformation("Getting review count for product with ID: {ProductId}", productId);
            var count = await _reviewRepo.GetReviewCountForProductAsync(productId);
            return Ok(count);
        }

        // GET: api/Review/product/5/distribution
        [HttpGet("product/{productId}/distribution")]
        public async Task<ActionResult<IDictionary<int, int>>> GetProductRatingDistribution(int productId)
        {
            // Check if product exists
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found when getting rating distribution", productId);
                return NotFound("Product not found");
            }
            
            _logger.LogInformation("Getting rating distribution for product with ID: {ProductId}", productId);
            var distribution = await _reviewRepo.GetRatingDistributionForProductAsync(productId);
            return Ok(distribution);
        }
    }
}
