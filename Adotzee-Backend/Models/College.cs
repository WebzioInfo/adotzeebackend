namespace Adotzee_Backend.Models
{
    public class College : BaseEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        // Real-world location fields
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Optional but powerful
        public string? GoogleMapsUrl { get; set; }
        public string? PlaceId { get; set; }

        public bool? IsRecommended { get; set; }
        public int DisplayOrder { get; set; } = 0;

        public ICollection<AddonCollege> AddonColleges { get; set; } = new List<AddonCollege>();
    }
}
