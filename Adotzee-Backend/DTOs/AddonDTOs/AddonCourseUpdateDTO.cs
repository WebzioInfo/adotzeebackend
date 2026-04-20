using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.AddonDTOs
{
    public class AddonCourseUpdateDTO : AddonCourseCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
