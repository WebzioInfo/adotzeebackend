using Adotzee_Backend.DTOs.UserDTOs;
using Adotzee_Backend.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Adotzee_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _userService.Login(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /* ---------------- REGISTER ---------------- */

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _userService.Register(dto);

            if (!result.Success)
                return Conflict(result);

            return StatusCode(201, result);
        }

        /* ---------------- GET ALL USERS ---------------- */

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsers();
            return Ok(result);
        }

        /* ---------------- GET USER ---------------- */

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserById(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /* ---------------- DELETE USER ---------------- */

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /* ---------------- BLOCK / UNBLOCK ---------------- */

        [Authorize(Roles = "Admin")]
        [HttpPatch("toggle-block/{id}")]
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var result = await _userService.ToggleBlockUser(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
