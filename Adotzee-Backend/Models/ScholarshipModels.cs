using System;
using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.Models
{
    public class Scholarship : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Provider { get; set; } = "Adotzee";

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = "Private Scholarship";

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Coming Soon"; // e.g., "Coming Soon", "Open", "Closed"

        [MaxLength(200)]
        public string Amount { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        // Storing as JSON string to keep simple without extra related tables initially
        public string EligibilityJson { get; set; } = "[]";

        [MaxLength(1000)]
        public string Disclaimer { get; set; } = string.Empty;

        public DateTime? ApplicationStartDate { get; set; }
        public DateTime? ApplicationEndDate { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        [MaxLength(500)]
        public string? BannerImageUrl { get; set; }

        public int TotalAvailable { get; set; } = 0;

        [MaxLength(2000)]
        public string? TermsAndConditions { get; set; }
    }

    public class ScholarshipEnquiry : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ScholarshipId { get; set; }

        public Scholarship? Scholarship { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PlusTwoPercentage { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string PreferredCourse { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? PreferredCollege { get; set; }
    }
}
