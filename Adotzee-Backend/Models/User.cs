using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Adotzee_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; } = "User";
    }
}
