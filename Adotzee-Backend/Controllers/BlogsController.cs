using Adotzee_Backend.DTOs.Blog;
using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;
using Adotzee_Backend.Services.BlogServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adotzee_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // Public endpoint to get published blogs with search/filter/pagination
        [HttpGet]
        public async Task<IActionResult> GetPublishedBlogs([FromQuery] BlogQueryParametersDto queryParameters)
        {
            // Force status to Published for public endpoint
            queryParameters.Status = BlogStatus.Published;

            var result = await _blogService.GetAllAsync(queryParameters);
            return Ok(ApiResponse<PagedResponse<BlogResponseDto>>.SuccessResponse(result, "Published blogs retrieved successfully."));
        }

        // Public endpoint to get featured blogs
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedBlogs([FromQuery] int count = 6)
        {
            var result = await _blogService.GetFeaturedAsync(count);
            return Ok(ApiResponse<List<BlogResponseDto>>.SuccessResponse(result, "Featured blogs retrieved successfully."));
        }

        // Public endpoint to get trending blogs
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingBlogs([FromQuery] int count = 6)
        {
            var result = await _blogService.GetTrendingAsync(count);
            return Ok(ApiResponse<List<BlogResponseDto>>.SuccessResponse(result, "Trending blogs retrieved successfully."));
        }

        // Public endpoint to get categories list
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _blogService.GetCategoriesAsync();
            return Ok(ApiResponse<List<string>>.SuccessResponse(result, "Categories retrieved successfully."));
        }

        // Public endpoint to get blog by slug
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBlogBySlug(string slug)
        {
            var result = await _blogService.GetBySlugAsync(slug);
            if (result == null || result.Status != BlogStatus.Published)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found or unpublished."));

            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog post retrieved successfully."));
        }

        // Public endpoint to get related blogs
        [HttpGet("related/{slug}")]
        public async Task<IActionResult> GetRelatedBlogs(string slug, [FromQuery] int count = 4)
        {
            var result = await _blogService.GetRelatedAsync(slug, count);
            return Ok(ApiResponse<List<BlogResponseDto>>.SuccessResponse(result, "Related blogs retrieved successfully."));
        }

        // Public endpoint to increment likes
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeBlog(int id)
        {
            var success = await _blogService.IncrementLikesAsync(id);
            if (!success)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<string>.SuccessResponse(string.Empty, "Blog liked successfully."));
        }

        // Admin endpoint to get all blogs (Draft, Published, Archived)
        [Authorize]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllBlogsAdmin([FromQuery] BlogQueryParametersDto queryParameters)
        {
            var result = await _blogService.GetAllAsync(queryParameters);
            return Ok(ApiResponse<PagedResponse<BlogResponseDto>>.SuccessResponse(result, "All blogs retrieved successfully."));
        }

        // Admin endpoint to get a single blog by ID
        [Authorize]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetBlogAdmin(int id)
        {
            var result = await _blogService.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog post retrieved successfully."));
        }

        // Admin endpoint to create a blog
        [Authorize]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blogService.CreateAsync(createDto);
            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog post created successfully."));
        }

        // Admin endpoint to update a blog
        [Authorize]
        [HttpPut("admin/{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] UpdateBlogDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blogService.UpdateAsync(id, updateDto);
            if (result == null)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog post updated successfully."));
        }

        // Admin endpoint to update blog status
        [Authorize]
        [HttpPut("admin/{id}/status")]
        public async Task<IActionResult> UpdateBlogStatus(int id, [FromBody] BlogStatus status)
        {
            var result = await _blogService.UpdateStatusAsync(id, status);
            if (result == null)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog status updated successfully."));
        }

        // Admin endpoint to toggle feature status
        [Authorize]
        [HttpPut("admin/{id}/feature")]
        public async Task<IActionResult> ToggleBlogFeature(int id, [FromBody] bool featured)
        {
            var result = await _blogService.ToggleFeatureAsync(id, featured);
            if (result == null)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<BlogResponseDto>.SuccessResponse(result, "Blog feature status updated successfully."));
        }

        // Admin endpoint to delete a blog
        [Authorize]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var success = await _blogService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<string>.FailResponse("Blog post not found."));

            return Ok(ApiResponse<string>.SuccessResponse(string.Empty, "Blog post deleted successfully."));
        }
    }
}
