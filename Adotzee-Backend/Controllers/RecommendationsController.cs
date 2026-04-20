using Adotzee_Backend.DTOs.RecommendationDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.RecommendationServices;
using Microsoft.AspNetCore.Mvc;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationsController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RecommendationResponseDTO>>> GetRecommendations([FromBody] RecommendationRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<RecommendationResponseDTO>.FailResponse("Request body cannot be null."));
            }
            if (string.IsNullOrWhiteSpace(request.Interests))
            {
                // Interests is required as per DTO definition
                return BadRequest(ApiResponse<RecommendationResponseDTO>.FailResponse("Interests must be provided for recommendations."));
            }

            var result = await _recommendationService.GetRecommendationsAsync(request);

            if (!result.Success)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}
