using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Review
{
    public class ReviewQueryParametersDto : PaginationParams
    {
        public ReviewStatus? Status { get; set; }
        public bool? Featured { get; set; }
        public int? Rating { get; set; }
        public string? Course { get; set; }
        public string? CollegeName { get; set; }
    }
}
