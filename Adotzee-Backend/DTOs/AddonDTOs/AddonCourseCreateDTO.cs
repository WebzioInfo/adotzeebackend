using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.AddonDTOs
{
    public class AddonCourseCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CourseId { get; set; }
        public List<int> CollegeIds { get; set; }
    }
}
