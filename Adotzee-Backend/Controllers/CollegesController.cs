using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Services.CollegeServices;
using Adotzee_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _service;

        public CollegesController(ICollegeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
            => Ok(await _service.GetPagedAsync(@params));

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUnpaginated()
            => Ok(await _service.GetAllUnpaginatedAsync());

        [Authorize(Roles = "Admin")]
        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<int> ids)
            => Ok(await _service.ReorderAsync(ids));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CollegeCreateDTO dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CollegeUpdateDTO dto)
        {
            return Ok(await _service.UpdateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.DeleteAsync(id));
    }
}
