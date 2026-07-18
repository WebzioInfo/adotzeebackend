using Adotzee_Backend.DTOs.Review;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.ReviewRepos
{
    public interface IReviewRepository
    {
        Task<PagedResponse<Review>> GetAllAsync(ReviewQueryParametersDto queryParameters);
        Task<List<Review>> GetFeaturedAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task DeleteAsync(Review review);
    }
}
