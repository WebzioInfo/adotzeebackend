using System.ComponentModel.DataAnnotations;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Review
{
    public class UpdateReviewDto
    {
        public ReviewStatus? Status { get; set; }
        
        public bool? Featured { get; set; }
        
        [MaxLength(255)]
        public string? DisplayName { get; set; }
        
        [MaxLength(10)]
        public string? DisplayInitials { get; set; }
        
        public VerificationType? VerificationType { get; set; }
    }
}
