using Adotzee_Backend.DTOs.Review;
using Adotzee_Backend.Services.ReviewServices;
using Adotzee_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adotzee_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Public endpoint to submit a review
        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] CreateReviewDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var result = await _reviewService.CreateAsync(createDto, ipAddress, userAgent);
            return Ok(ApiResponse<ReviewResponseDto>.SuccessResponse(result, "Review submitted successfully and is pending approval."));
        }

        // Public endpoint to get featured reviews
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedReviews()
        {
            var result = await _reviewService.GetFeaturedAsync();
            return Ok(ApiResponse<List<ReviewResponseDto>>.SuccessResponse(result, "Featured reviews retrieved successfully."));
        }

        // Public endpoint to get approved reviews with pagination/filters
        [HttpGet]
        public async Task<IActionResult> GetApprovedReviews([FromQuery] ReviewQueryParametersDto queryParameters)
        {
            // Force status to Approved for public endpoint
            queryParameters.Status = Models.Enums.ReviewStatus.Approved;
            
            var result = await _reviewService.GetAllAsync(queryParameters);
            return Ok(ApiResponse<PagedResponse<ReviewResponseDto>>.SuccessResponse(result, "Approved reviews retrieved successfully."));
        }

        // Admin endpoint to get all reviews
        [Authorize]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllReviewsAdmin([FromQuery] ReviewQueryParametersDto queryParameters)
        {
            var result = await _reviewService.GetAllAsync(queryParameters);
            return Ok(ApiResponse<PagedResponse<ReviewResponseDto>>.SuccessResponse(result, "Reviews retrieved successfully."));
        }

        // Admin endpoint to get a single review by id
        [Authorize]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetReviewAdmin(int id)
        {
            var result = await _reviewService.GetByIdAsync(id);
            if (result == null) return NotFound(ApiResponse<string>.FailResponse("Review not found."));
            
            return Ok(ApiResponse<ReviewResponseDto>.SuccessResponse(result, "Review retrieved successfully."));
        }

        // Admin endpoint to update review status
        [Authorize]
        [HttpPut("admin/{id}/status")]
        public async Task<IActionResult> UpdateReviewStatus(int id, [FromBody] UpdateReviewDto updateDto)
        {
            var approvedBy = User.Identity?.Name ?? "Admin";
            var result = await _reviewService.UpdateStatusAsync(id, updateDto, approvedBy);
            
            if (result == null) return NotFound(ApiResponse<string>.FailResponse("Review not found."));
            
            return Ok(ApiResponse<ReviewResponseDto>.SuccessResponse(result, "Review status updated successfully."));
        }

        // Admin endpoint to toggle feature status
        [Authorize]
        [HttpPut("admin/{id}/feature")]
        public async Task<IActionResult> ToggleReviewFeature(int id, [FromBody] bool featured)
        {
            var result = await _reviewService.ToggleFeatureAsync(id, featured);
            
            if (result == null) return NotFound(ApiResponse<string>.FailResponse("Review not found."));
            
            return Ok(ApiResponse<ReviewResponseDto>.SuccessResponse(result, "Review feature status updated successfully."));
        }

        // Admin endpoint to delete a review
        [Authorize]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var success = await _reviewService.DeleteAsync(id);
            if (!success) return NotFound(ApiResponse<string>.FailResponse("Review not found."));
            
            return Ok(ApiResponse<string>.SuccessResponse(string.Empty, "Review deleted successfully."));
        }
    }
}
