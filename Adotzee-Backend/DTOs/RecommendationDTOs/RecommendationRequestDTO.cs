namespace Adotzee_Backend.DTOs.RecommendationDTOs
{
    public class RecommendationRequestDTO
    {
        public string Interests { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? PreferredStream { get; set; }
        public string? PreferredCourseType { get; set; }
        public string? PreferredDuration { get; set; }
    }
}
