namespace Adotzee_Backend.DTOs.CourseDTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public string Stream { get; set; }
        public int DisplayOrder { get; set; }
    }
}
