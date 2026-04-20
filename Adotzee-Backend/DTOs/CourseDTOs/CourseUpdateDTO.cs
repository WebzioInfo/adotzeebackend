using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.CourseDTOs
{
    public class CourseUpdateDTO : CourseCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
