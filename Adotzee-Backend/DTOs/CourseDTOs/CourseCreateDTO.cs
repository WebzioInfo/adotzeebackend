using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.CourseDTOs
{
    public class CourseCreateDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Duration { get; set; }
        [Required]
        public required string Type { get; set; }  // "UG", "PG"
        [Required]
        public required string Stream { get; set; }
    }
}
