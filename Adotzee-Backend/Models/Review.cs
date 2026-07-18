using System.ComponentModel.DataAnnotations;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.Models
{
    public class Review : BaseEntity
    {
        public int Id { get; set; }
        
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
        
        public VerificationType VerificationType { get; set; } = VerificationType.None;
        
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
        
        public bool Featured { get; set; } = false;
        
        [MaxLength(255)]
        public string? DisplayName { get; set; }
        
        [MaxLength(10)]
        public string? DisplayInitials { get; set; }
        
        public bool IsAnonymous { get; set; } = false;
        
        public DateTime? ApprovedAt { get; set; }
        
        [MaxLength(255)]
        public string? ApprovedBy { get; set; }
        
        [MaxLength(50)]
        public string? IpAddress { get; set; }
        
        [MaxLength(1000)]
        public string? UserAgent { get; set; }
    }
}
