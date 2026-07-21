using System;
using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.ScholarshipDTOs
{
    public class ScholarshipDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EligibilityJson { get; set; } = string.Empty;
        public string Disclaimer { get; set; } = string.Empty;
        public DateTime? ApplicationStartDate { get; set; }
        public DateTime? ApplicationEndDate { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? TermsAndConditions { get; set; }
    }

    public class CreateScholarshipEnquiryDTO
    {
        [Required]
        public Guid ScholarshipId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
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
