using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Services.AddonsServices;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder([FromBody] List<int> ids) =>
        Ok(await _service.ReorderAsync(ids));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddonCourseCreateDTO dto) =>
        Ok(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AddonCourseUpdateDTO dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");
        return Ok(await _service.UpdateAsync(dto));
    }

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
