namespace Adotzee_Backend.DTOs.AddonDTOs
{
    public class AddonCourseResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CourseName { get; set; }
        public List<string> CollegeNames { get; set; } = new();
        public int DisplayOrder { get; set; }
    }
}
