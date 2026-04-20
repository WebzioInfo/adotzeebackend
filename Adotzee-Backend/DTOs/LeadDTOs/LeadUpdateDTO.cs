using System.ComponentModel.DataAnnotations;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.DTOs.LeadDTOs
{
    public class LeadUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? CourseInterested { get; set; }

        public string? CollegeInterested { get; set; }

        [Required]
        public LeadSource Source { get; set; }

        [Required]
        public LeadStatus Status { get; set; }

        public LeadPriority Priority { get; set; }

        public string? Notes { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public int? AssignedToUserId { get; set; }
    }
}
