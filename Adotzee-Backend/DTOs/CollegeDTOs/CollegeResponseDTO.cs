namespace Adotzee_Backend.DTOs.CollegeDTOs
{
    public class CollegeResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? GoogleMapsUrl { get; set; }
        public string? PlaceId { get; set; }

        public bool IsRecommended { get; set; }
        public int DisplayOrder { get; set; }

        public List<string> Addons { get; set; } = new();
    }
}
