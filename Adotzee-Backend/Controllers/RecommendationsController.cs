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
        private readonly ILogger<RecommendationsController> _logger;

        public RecommendationsController(IRecommendationService recommendationService, ILogger<RecommendationsController> logger)
        {
            _recommendationService = recommendationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RecommendationResponseDTO>>> GetRecommendations([FromBody] RecommendationRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<RecommendationResponseDTO>.FailResponse("Request body cannot be null."));
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<RecommendationResponseDTO>.FailResponse($"Validation failed: {errors}"));
            }

            if (string.IsNullOrWhiteSpace(request.Interests))
            {
                return BadRequest(ApiResponse<RecommendationResponseDTO>.FailResponse("Interests must be provided for recommendations."));
            }

            try
            {
                var result = await _recommendationService.GetRecommendationsAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning($"Recommendation service failed: {result.Message}");
                    return StatusCode(500, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while generating recommendations.");
                return StatusCode(500, ApiResponse<RecommendationResponseDTO>.FailResponse(
                    "Recommendation service is temporarily unavailable.", 
                    "SERVICE_CONFIGURATION_ERROR"
                ));
            }
        }
    }
}
