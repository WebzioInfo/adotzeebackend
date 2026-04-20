namespace Adotzee_Backend.Models
{
    public class AddonCollege
    {
        public int Id { get; set; }

        public int AddonCourseId { get; set; }
        public AddonCourse AddonCourse { get; set; }

        public int CollegeId { get; set; }
        public College College { get; set; }
    }
}
