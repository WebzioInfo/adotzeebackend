using Adotzee_Backend.DTOs.CourseDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.DTOs.SearchDTOs
{
    public class GlobalSearchResponseDTO
    {
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public IEnumerable<College> Colleges { get; set; } = new List<College>();
        public IEnumerable<AddonCourse> Addons { get; set; } = new List<AddonCourse>();
    }
}
