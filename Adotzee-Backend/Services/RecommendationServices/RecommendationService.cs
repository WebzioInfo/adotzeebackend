using Adotzee_Backend.DTOs.RecommendationDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.RecommendationRepos;
using Microsoft.Extensions.Caching.Memory;

namespace Adotzee_Backend.Services.RecommendationServices
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _repo;
        private readonly IMemoryCache _cache;

        public RecommendationService(IRecommendationRepository repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ApiResponse<RecommendationResponseDTO>> GetRecommendationsAsync(RecommendationRequestDTO request)
        {
            // Simple cache key generation
            string cacheKey = $"recommendations_{request.Interests}_{request.Location}_{request.PreferredStream}_{request.PreferredCourseType}_{request.PreferredDuration}";
            
            if (!_cache.TryGetValue(cacheKey, out RecommendationResponseDTO? responseDto))
            {
                var (courses, colleges, addons) = await _repo.GetRecommendationsAsync(request);

                responseDto = new RecommendationResponseDTO
                {
                    RecommendedCourses = courses,
                    RecommendedColleges = colleges,
                    RecommendedAddons = addons
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                _cache.Set(cacheKey, responseDto, cacheOptions);
            }

            return ApiResponse<RecommendationResponseDTO>.SuccessResponse(responseDto!, "Recommendations generated successfully");
        }
    }
}