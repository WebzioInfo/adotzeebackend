using Adotzee_Backend.DTOs.ScholarshipDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.ScholarshipServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adotzee_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScholarshipsController : ControllerBase
    {
        private readonly IScholarshipService _scholarshipService;

        public ScholarshipsController(IScholarshipService scholarshipService)
        {
            _scholarshipService = scholarshipService;
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScholarshipDTO>>>> GetActiveScholarships()
        {
            var response = await _scholarshipService.GetActiveScholarshipsAsync();
            return Ok(response);
        }

        [HttpPost("apply")]
        public async Task<ActionResult<ApiResponse<string>>> ApplyForScholarship([FromBody] CreateScholarshipEnquiryDTO applicationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailResponse("Invalid application data."));
            }

            var response = await _scholarshipService.ApplyForScholarshipAsync(applicationDto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
