using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.Services.AddonsServices;
using Adotzee_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]

public class AddonsController : ControllerBase
{
    private readonly IAddonsService _service;

    public AddonsController(IAddonsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params) =>
        Ok(await _service.GetPagedAsync(@params));

    [Authorize(Roles = "Admin")]
    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder([FromBody] List<int> ids) =>
        Ok(await _service.ReorderAsync(ids));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) =>
        Ok(await _service.GetByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddonCourseCreateDTO dto) =>
        Ok(await _service.CreateAsync(dto));

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AddonCourseUpdateDTO dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");
        return Ok(await _service.UpdateAsync(dto));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await _service.DeleteAsync(id));

    [HttpGet("{id}/colleges")]
    public async Task<IActionResult> GetCollegesForAddon(int id)
    {
        var result = await _service.GetCollegesByAddonIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("by-course/{courseId}")]
    public async Task<IActionResult> GetByCourseId(int courseId)
    {
        var result = await _service.GetByCourseIdAsync(courseId);
        return Ok(result);
    }
}
