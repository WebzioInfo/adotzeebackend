using Adotzee_Backend.Models.Enums;

namespace Adotzee_Backend.DTOs.Blog
{
    public class BlogResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public string? Excerpt { get; set; }
        public required string Content { get; set; }
        public string? CoverImage { get; set; }
        public string? CoverImageAlt { get; set; }
        public string? BannerImage { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorRole { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public BlogStatus Status { get; set; }
        public bool Featured { get; set; }
        public int ReadTimeMinutes { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int SharesCount { get; set; }
        public string? SeoTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? CanonicalUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
