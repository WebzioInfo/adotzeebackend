using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.DTOs.CollegeDTOs
{
    public class CollegeCreateDTO
    {
        [Required]
        public string Name { get; set; }


        [Required]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string? GoogleMapsUrl { get; set; }
        public string? PlaceId { get; set; }

        public bool IsRecommended { get; set; }

        public List<int>? AddonIds { get; set; }
    }
}
