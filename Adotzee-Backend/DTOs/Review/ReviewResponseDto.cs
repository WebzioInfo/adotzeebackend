using System;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Course { get; set; } = string.Empty;
        public string? CollegeName { get; set; }
        public int Rating { get; set; }
        public string ReviewTitle { get; set; } = string.Empty;
        public string ReviewMessage { get; set; } = string.Empty;
        public string? StudentPhoto { get; set; }
        public string VerificationType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool Featured { get; set; }
        public string? DisplayName { get; set; }
        public string? DisplayInitials { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
