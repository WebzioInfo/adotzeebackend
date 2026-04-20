using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.CollegeServices;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<int> ids)
            => Ok(await _service.ReorderAsync(ids));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CollegeCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CollegeUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.DeleteAsync(id));
    }
}
