using System.ComponentModel.DataAnnotations;

namespace Adotzee_Backend.Models
{
    public class AptitudeCategory : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }
    }

    public class AptitudeQuestion : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Text { get; set; }

        public int CategoryId { get; set; }
        public AptitudeCategory? Category { get; set; }

        public int Weightage { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }

    public class AssessmentResult : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LeadId { get; set; }
        public Lead? Lead { get; set; }

        public string? ResultJson { get; set; }
    }
}
