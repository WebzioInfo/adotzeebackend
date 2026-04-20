using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.UserDTOs
{
    public class RegisterDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "password and confirmation password do not match.")]
        public required string CPassword { get; set; }
    }
}
