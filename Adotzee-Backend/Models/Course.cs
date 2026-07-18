using Adotzee_Backend.Helpers;

namespace Adotzee_Backend.Models
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public CourseType Type { get; set; } // UG / PG
        public StreamType Stream { get; set; } // Science, Arts, etc.
        public required string Duration { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public ICollection<AddonCourse> AddonCourses { get; set; } = new List<AddonCourse>();
    }
    
}
