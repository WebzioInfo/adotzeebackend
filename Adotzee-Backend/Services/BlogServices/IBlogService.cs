using Adotzee_Backend.DTOs.Blog;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.BlogServices
{
    public interface IBlogService
    {
        Task<PagedResponse<BlogResponseDto>> GetAllAsync(BlogQueryParametersDto queryParams);
        Task<BlogResponseDto?> GetByIdAsync(int id);
        Task<BlogResponseDto?> GetBySlugAsync(string slug);
        Task<List<BlogResponseDto>> GetFeaturedAsync(int count = 6);
        Task<List<BlogResponseDto>> GetTrendingAsync(int count = 6);
        Task<List<BlogResponseDto>> GetRelatedAsync(string slug, int count = 4);
        Task<List<string>> GetCategoriesAsync();
        Task<BlogResponseDto> CreateAsync(CreateBlogDto createDto);
        Task<BlogResponseDto?> UpdateAsync(int id, UpdateBlogDto updateDto);
        Task<BlogResponseDto?> UpdateStatusAsync(int id, Models.Enums.BlogStatus status);
        Task<BlogResponseDto?> ToggleFeatureAsync(int id, bool featured);
        Task<bool> IncrementViewsAsync(int id);
        Task<bool> IncrementLikesAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
