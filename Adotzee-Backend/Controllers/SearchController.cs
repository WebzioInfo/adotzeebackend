using Adotzee_Backend.DTOs.SearchDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<GlobalSearchResponseDTO>>> GlobalSearch([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(ApiResponse<GlobalSearchResponseDTO>.FailResponse("Query parameter 'q' is required."));
            }

            var result = await _searchService.GlobalSearchAsync(q);

            if (!result.Success)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}
