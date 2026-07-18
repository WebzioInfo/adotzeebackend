namespace Adotzee_Backend.Models
{
    public class AddonCourse : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
 
        public int CourseId { get; set; }
        public Course Course { get; set; } = default!;
        public int DisplayOrder { get; set; } = 0;
 
        public ICollection<AddonCollege> AddonColleges { get; set; } = new List<AddonCollege>();
    }

}
