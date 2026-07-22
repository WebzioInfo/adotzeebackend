using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Blog
{
    public class BlogQueryParametersDto : PaginationParams
    {
        public string? Category { get; set; }
        public string? Tag { get; set; }
        public BlogStatus? Status { get; set; }
        public bool? Featured { get; set; }
        public string? SortBy { get; set; } // "latest", "popular", "trending"
    }
}
