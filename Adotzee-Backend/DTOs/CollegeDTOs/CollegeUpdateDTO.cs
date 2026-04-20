using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.CollegeDTOs
{
    public class CollegeUpdateDTO : CollegeCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
