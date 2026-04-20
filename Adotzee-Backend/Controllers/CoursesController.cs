using Adotzee_Backend.DTOs.CourseDTOs;
using Adotzee_Backend.Services.CourseServices;
using Adotzee_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
        {
            var result = await _service.GetPagedAsync(@params);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<int> ids)
        {
            var result = await _service.ReorderAsync(ids);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseUpdateDTO dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterByTypeStream([FromQuery] string? type, [FromQuery] string? stream)
        {
            var result = await _service.FilterByTypeStreamAsync(type ?? "", stream ?? "");
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats() => Ok(await _service.GetDashboardStats());

        [HttpGet("{id}/addons")]
        public async Task<IActionResult> GetAddonsForCourse(int id)
        {
            var result = await _service.GetAddonCoursesByCourseIdAsync(id);
            return Ok(result);
        }

    }
}
