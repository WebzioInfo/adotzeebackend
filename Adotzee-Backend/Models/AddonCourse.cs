namespace Adotzee_Backend.Models
{
    public class AddonCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int DisplayOrder { get; set; }

        public ICollection<AddonCollege> AddonColleges { get; set; }
    }

}
