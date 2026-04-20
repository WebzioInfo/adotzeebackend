using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.UserDTOs
{
    public class LoginDTO
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
