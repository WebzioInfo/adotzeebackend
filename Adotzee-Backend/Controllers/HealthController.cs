using Microsoft.AspNetCore.Mvc;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly Data.AppDbContext _context;

        public HealthController(Data.AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new 
                { 
                    status = canConnect ? "Healthy" : "Unhealthy", 
                    database = canConnect ? "Connected" : "Disconnected",
                    timestamp = DateTime.UtcNow 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Unhealthy", error = ex.Message, timestamp = DateTime.UtcNow });
            }
        }
    }
}
