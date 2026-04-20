using Adotzee_Backend.Helpers;

namespace Adotzee_Backend.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseType Type { get; set; } // UG / PG
        public StreamType Stream { get; set; } // Science, Arts, etc.
        public string Duration { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<AddonCourse> AddonCourses { get; set; }
    }
    
}
