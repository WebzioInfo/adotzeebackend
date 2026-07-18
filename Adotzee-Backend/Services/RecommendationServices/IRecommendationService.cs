using Adotzee_Backend.DTOs.RecommendationDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.RecommendationServices
{
    public interface IRecommendationService
    {
        Task<ApiResponse<RecommendationResponseDTO>> GetRecommendationsAsync(RecommendationRequestDTO request);
    }
}