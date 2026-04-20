using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.LeadServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _service;

        public LeadsController(ILeadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? cursor = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? source = null,
            [FromQuery] string? status = null)
        {
            return Ok(await _service.GetAllAsync(cursor, pageSize, search, source, status));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeadCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LeadUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.DeleteAsync(id));

        [HttpPatch("{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, LeadStatus status)
        {
            return Ok(await _service.UpdateStatusAsync(id, status));
        }

        // --- DASHBOARD ENDPOINTS ---

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
            => Ok(await _service.GetDashboardStatsAsync());
            
        [HttpGet("source-breakdown")]
        public async Task<IActionResult> GetSourceBreakdown()
        {
            var stats = await _service.GetDashboardStatsAsync();
            if (stats.Success && stats.Data != null)
                return Ok(ApiResponse<Dictionary<string, int>>.SuccessResponse(stats.Data.LeadsBySource));
            
            return BadRequest(ApiResponse<string>.FailResponse("Failed to fetch source breakdown"));
        }

        [HttpGet("status-breakdown")]
        public async Task<IActionResult> GetStatusBreakdown()
        {
            var stats = await _service.GetDashboardStatsAsync();
            if (stats.Success && stats.Data != null)
                return Ok(ApiResponse<Dictionary<string, int>>.SuccessResponse(stats.Data.LeadsByStatus));
            
            return BadRequest(ApiResponse<string>.FailResponse("Failed to fetch status breakdown"));
        }
        
        [HttpGet("monthly-trend")]
        public async Task<IActionResult> GetMonthlyTrend()
        {
            var stats = await _service.GetDashboardStatsAsync();
            if (stats.Success && stats.Data != null)
                return Ok(ApiResponse<List<MonthWiseDTO>>.SuccessResponse(stats.Data.MonthlyLeads));
            
            return BadRequest(ApiResponse<string>.FailResponse("Failed to fetch monthly trend"));
        }
    }
}
