using Adotzee_Backend.Models;

namespace Adotzee_Backend.DTOs.LeadDTOs
{
    public class LeadResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? CourseInterested { get; set; }
        public string? CollegeInterested { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
