using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.Review
{
    public class CreateReviewDto
    {
        [Required]
        [MaxLength(255)]
        public required string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }
        
        [MaxLength(20)]
        public string? MobileNumber { get; set; }
        
        [MaxLength(100)]
        public string? City { get; set; }
        
        [MaxLength(100)]
        public string? State { get; set; }
        
        [Required]
        [MaxLength(255)]
        public required string Course { get; set; }
        
        [MaxLength(255)]
        public string? CollegeName { get; set; }
        
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [MaxLength(500)]
        public required string ReviewTitle { get; set; }
        
        [Required]
        public required string ReviewMessage { get; set; }
        
        public string? StudentPhoto { get; set; }
        
        public bool IsAnonymous { get; set; }
    }
}
