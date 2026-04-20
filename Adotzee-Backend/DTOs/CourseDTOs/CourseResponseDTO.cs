namespace Adotzee_Backend.DTOs.CourseDTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Duration { get; set; }
        public required string Type { get; set; }
        public required string Stream { get; set; }
        public int DisplayOrder { get; set; }
    }
}
