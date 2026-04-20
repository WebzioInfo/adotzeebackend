namespace Adotzee_Backend.DTOs.AddonDTOs
{
    public class AddonCourseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public List<string> CollegeNames { get; set; }
        public int DisplayOrder { get; set; }
    }
}
