using Store.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.I_Repos
{
    public interface I_ReviewRepo
    {
        // Synchronous operations with IQueryable for LINQ
        IQueryable<Review> GetAllReviews();
        IQueryable<Review> GetReviewsByProductId(int productId);
        IQueryable<Review> GetReviewsByUserId(int userId);
        
        // Asynchronous operations
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review> AddReviewAsync(Review review, int userId);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int id);
        
        // Statistics and analytics
        Task<double> GetAverageRatingForProductAsync(int productId);
        Task<int> GetReviewCountForProductAsync(int productId);
        Task<IDictionary<int, int>> GetRatingDistributionForProductAsync(int productId);
    }
}
