using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.RecommendationDTOs
{
    public class RecommendationRequestDTO
    {
        [Required(ErrorMessage = "Interests are required")]
        [MaxLength(500, ErrorMessage = "Interests cannot exceed 500 characters")]
        public string Interests { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? PreferredStream { get; set; }

        [MaxLength(50)]
        public string? PreferredCourseType { get; set; }

        [MaxLength(50)]
        public string? PreferredDuration { get; set; }
    }
}
