using Adotzee_Backend.DTOs.Review;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<PagedResponse<ReviewResponseDto>> GetAllAsync(ReviewQueryParametersDto queryParameters);
        Task<List<ReviewResponseDto>> GetFeaturedAsync();
        Task<ReviewResponseDto?> GetByIdAsync(int id);
        Task<ReviewResponseDto> CreateAsync(CreateReviewDto createReviewDto, string? ipAddress, string? userAgent);
        Task<ReviewResponseDto?> UpdateStatusAsync(int id, UpdateReviewDto updateDto, string approvedBy);
        Task<ReviewResponseDto?> ToggleFeatureAsync(int id, bool featured);
        Task<bool> DeleteAsync(int id);
    }
}
