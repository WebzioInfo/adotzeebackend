using System.ComponentModel.DataAnnotations;
using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Blog
{
    public class CreateBlogDto
    {
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [MaxLength(255)]
        public string? Slug { get; set; }

        [MaxLength(500)]
        public string? Excerpt { get; set; }

        [Required]
        public required string Content { get; set; }

        [MaxLength(1000)]
        public string? CoverImage { get; set; }

        [MaxLength(255)]
        public string? CoverImageAlt { get; set; }

        [MaxLength(1000)]
        public string? BannerImage { get; set; }

        [MaxLength(100)]
        public string? AuthorName { get; set; }

        [MaxLength(100)]
        public string? AuthorRole { get; set; }

        [MaxLength(1000)]
        public string? AuthorAvatar { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(255)]
        public string? Tags { get; set; }

        public BlogStatus Status { get; set; } = BlogStatus.Draft;

        public bool Featured { get; set; } = false;

        public int ReadTimeMinutes { get; set; } = 5;

        [MaxLength(255)]
        public string? SeoTitle { get; set; }

        [MaxLength(500)]
        public string? MetaDescription { get; set; }

        [MaxLength(500)]
        public string? CanonicalUrl { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime? ScheduledAt { get; set; }
    }
}
