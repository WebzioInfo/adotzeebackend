using Adotzee_Backend.Models;

namespace Adotzee_Backend.DTOs.RecommendationDTOs
{
    public class RecommendationResponseDTO
    {
        public IEnumerable<Course> RecommendedCourses { get; set; } = new List<Course>();
        public IEnumerable<College> RecommendedColleges { get; set; } = new List<College>();
        public IEnumerable<AddonCourse> RecommendedAddons { get; set; } = new List<AddonCourse>();
    }
}
