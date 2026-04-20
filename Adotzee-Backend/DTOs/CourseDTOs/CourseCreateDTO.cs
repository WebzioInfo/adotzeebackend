using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.CourseDTOs
{
    public class CourseCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Duration { get; set; }
        [Required]
        public string Type { get; set; }  // "UG", "PG"
        [Required]
        public string Stream { get; set; }
    }
}
