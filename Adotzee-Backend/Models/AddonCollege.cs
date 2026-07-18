namespace Adotzee_Backend.Models
{
    public class AddonCollege : BaseEntity
    {
        public int Id { get; set; }

        public int AddonCourseId { get; set; }
        public AddonCourse AddonCourse { get; set; } = default!;

        public int CollegeId { get; set; }
        public College College { get; set; } = default!;
    }
}
