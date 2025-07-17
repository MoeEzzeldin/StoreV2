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
    public class ReviewRepo : I_ReviewRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IDapperContext _dapperContext;

        public ReviewRepo(ApplicationDbContext context, IDapperContext dapperContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        #region IQueryable Methods (Entity Framework)
        
        public IQueryable<Review> GetAllReviews()
        {
            return _context.Reviews.AsQueryable();
        }

        public IQueryable<Review> GetReviewsByProductId(int productId)
        {
            return _context.Reviews
                .Where(r => r.ProductId == productId)
                .AsQueryable();
        }

        public IQueryable<Review> GetReviewsByUserId(int userId)
        {
            return _context.Reviews
                .Where(r => r.Users.Any(u => u.UserId == userId))
                .AsQueryable();
        }

        #endregion

        #region Async Methods (Mix of EF Core and Dapper)

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Review>("SELECT * FROM review");
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var review = await connection.QuerySingleOrDefaultAsync<Review>(
                "SELECT * FROM review WHERE review_id = @Id", 
                new { Id = id });
                
            if (review != null)
            {
                // Get associated users
                var users = await connection.QueryAsync<User>(
                    @"SELECT u.* FROM users u
                      JOIN user_review ur ON u.user_id = ur.user_id
                      WHERE ur.review_id = @ReviewId",
                    new { ReviewId = id });
                
                review.Users = users.ToList();
            }
            
            return review;
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Review>(
                "SELECT * FROM review WHERE product_id = @ProductId", 
                new { ProductId = productId });
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<Review>(
                @"SELECT r.* FROM review r
                  JOIN user_review ur ON r.review_id = ur.review_id
                  WHERE ur.user_id = @UserId", 
                new { UserId = userId });
        }

        public async Task<Review> AddReviewAsync(Review review, int userId)
        {
            // Use a transaction to ensure all operations complete successfully
            using var connection = _dapperContext.CreateConnection();
            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            try
            {
                // Set the review date if not already set
                if (review.Date == default)
                {
                    review.Date = DateTime.UtcNow;
                }
                
                // Insert the review
                var reviewId = await connection.ExecuteScalarAsync<int>(
                    @"INSERT INTO review (product_id, reviewer, rating, title, comment, date) 
                      VALUES (@ProductId, @Reviewer, @Rating, @Title, @Comment, @Date);
                      SELECT CAST(SCOPE_IDENTITY() as int)",
                    new { 
                        review.ProductId,
                        review.Reviewer,
                        review.Rating,
                        review.Title,
                        review.Comment,
                        review.Date
                    },
                    transaction);
                
                review.ReviewId = reviewId;
                
                // Create association with the user
                await connection.ExecuteAsync(
                    @"INSERT INTO user_review (review_id, user_id) 
                      VALUES (@ReviewId, @UserId)",
                    new { ReviewId = reviewId, UserId = userId },
                    transaction);
                
                transaction.Commit();
                return review;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            using var connection = _dapperContext.CreateConnection();
            await connection.ExecuteAsync(
                @"UPDATE review SET 
                    product_id = @ProductId,
                    reviewer = @Reviewer,
                    rating = @Rating,
                    title = @Title,
                    comment = @Comment,
                    date = @Date
                  WHERE review_id = @ReviewId",
                new { 
                    review.ProductId,
                    review.Reviewer,
                    review.Rating,
                    review.Title,
                    review.Comment,
                    review.Date,
                    review.ReviewId
                });
            
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            // Use a transaction to ensure all operations complete successfully
            using var connection = _dapperContext.CreateConnection();
            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            try
            {
                // Delete associations in user_review table
                await connection.ExecuteAsync(
                    "DELETE FROM user_review WHERE review_id = @Id",
                    new { Id = id },
                    transaction);
                    
                // Delete the review
                var affectedRows = await connection.ExecuteAsync(
                    "DELETE FROM review WHERE review_id = @Id",
                    new { Id = id },
                    transaction);
                
                transaction.Commit();
                return affectedRows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteScalarAsync<double?>(
                "SELECT AVG(CAST(rating AS FLOAT)) FROM review WHERE product_id = @ProductId",
                new { ProductId = productId });
                
            return result ?? 0;
        }

        public async Task<int> GetReviewCountForProductAsync(int productId)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM review WHERE product_id = @ProductId",
                new { ProductId = productId });
        }

        public async Task<IDictionary<int, int>> GetRatingDistributionForProductAsync(int productId)
        {
            using var connection = _dapperContext.CreateConnection();
            var rows = await connection.QueryAsync<(int Rating, int Count)>(
                @"SELECT rating, COUNT(*) as Count 
                  FROM review 
                  WHERE product_id = @ProductId 
                  GROUP BY rating",
                new { ProductId = productId });
                
            // Convert to dictionary
            return rows.ToDictionary(row => row.Rating, row => row.Count);
        }

        #endregion
    }
}
