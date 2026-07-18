using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.Models
{
    public class Lead : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? CourseInterested { get; set; }

        public string? CollegeInterested { get; set; }

        [Required]
        public LeadSource Source { get; set; }

        [Required]
        public LeadStatus Status { get; set; } = LeadStatus.New;

        public LeadPriority Priority { get; set; } = LeadPriority.Medium;

        public string? Notes { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

    }
}
