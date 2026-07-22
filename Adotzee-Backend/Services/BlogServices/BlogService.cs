using System.Text.RegularExpressions;
using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs.Blog;
using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Services.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<BlogResponseDto>> GetAllAsync(BlogQueryParametersDto queryParams)
        {
            var query = _context.Blogs.AsNoTracking().AsQueryable();

            // Status Filter
            if (queryParams.Status.HasValue)
            {
                query = query.Where(b => b.Status == queryParams.Status.Value);
            }

            // Category Filter
            if (!string.IsNullOrWhiteSpace(queryParams.Category))
            {
                query = query.Where(b => b.Category != null && b.Category.ToLower() == queryParams.Category.ToLower());
            }

            // Tag Filter
            if (!string.IsNullOrWhiteSpace(queryParams.Tag))
            {
                query = query.Where(b => b.Tags != null && b.Tags.ToLower().Contains(queryParams.Tag.ToLower()));
            }

            // Featured Filter
            if (queryParams.Featured.HasValue)
            {
                query = query.Where(b => b.Featured == queryParams.Featured.Value);
            }

            // Search Keyword
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var term = queryParams.Search.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    (b.Excerpt != null && b.Excerpt.ToLower().Contains(term)) ||
                    (b.Content != null && b.Content.ToLower().Contains(term)) ||
                    (b.Tags != null && b.Tags.ToLower().Contains(term)) ||
                    (b.AuthorName != null && b.AuthorName.ToLower().Contains(term))
                );
            }

            // Sorting
            query = (queryParams.SortBy?.ToLower()) switch
            {
                "popular" => query.OrderByDescending(b => b.ViewsCount).ThenByDescending(b => b.CreatedAt),
                "trending" => query.OrderByDescending(b => b.LikesCount).ThenByDescending(b => b.ViewsCount),
                _ => query.OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var pageNumber = !queryParams.PageNumber.HasValue || queryParams.PageNumber.Value <= 0 ? 1 : queryParams.PageNumber.Value;
            var pageSize = !queryParams.PageSize.HasValue || queryParams.PageSize.Value <= 0 ? 10 : queryParams.PageSize.Value;

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => MapToDto(b))
                .ToListAsync();

            return new PagedResponse<BlogResponseDto>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<BlogResponseDto?> GetByIdAsync(int id)
        {
            var blog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            return blog == null ? null : MapToDto(blog);
        }

        public async Task<BlogResponseDto?> GetBySlugAsync(string slug)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Slug.ToLower() == slug.ToLower());
            if (blog == null) return null;

            // Increment view count
            blog.ViewsCount += 1;
            await _context.SaveChangesAsync();

            return MapToDto(blog);
        }

        public async Task<List<BlogResponseDto>> GetFeaturedAsync(int count = 6)
        {
            var blogs = await _context.Blogs.AsNoTracking()
                .Where(b => b.Status == BlogStatus.Published && b.Featured)
                .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
                .Take(count)
                .Select(b => MapToDto(b))
                .ToListAsync();

            return blogs;
        }

        public async Task<List<BlogResponseDto>> GetTrendingAsync(int count = 6)
        {
            var blogs = await _context.Blogs.AsNoTracking()
                .Where(b => b.Status == BlogStatus.Published)
                .OrderByDescending(b => b.ViewsCount)
                .ThenByDescending(b => b.LikesCount)
                .Take(count)
                .Select(b => MapToDto(b))
                .ToListAsync();

            return blogs;
        }

        public async Task<List<BlogResponseDto>> GetRelatedAsync(string slug, int count = 4)
        {
            var current = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Slug.ToLower() == slug.ToLower());
            if (current == null) return new List<BlogResponseDto>();

            var related = await _context.Blogs.AsNoTracking()
                .Where(b => b.Id != current.Id && b.Status == BlogStatus.Published && (b.Category == current.Category || (b.Tags != null && current.Tags != null && b.Tags.Contains(current.Tags))))
                .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
                .Take(count)
                .Select(b => MapToDto(b))
                .ToListAsync();

            if (related.Count < count)
            {
                var extra = await _context.Blogs.AsNoTracking()
                    .Where(b => b.Id != current.Id && b.Status == BlogStatus.Published && !related.Select(r => r.Id).Contains(b.Id))
                    .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
                    .Take(count - related.Count)
                    .Select(b => MapToDto(b))
                    .ToListAsync();
                related.AddRange(extra);
            }

            return related;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var categories = await _context.Blogs.AsNoTracking()
                .Where(b => b.Status == BlogStatus.Published && !string.IsNullOrEmpty(b.Category))
                .Select(b => b.Category!)
                .Distinct()
                .ToListAsync();

            return categories;
        }

        public async Task<BlogResponseDto> CreateAsync(CreateBlogDto createDto)
        {
            var slug = string.IsNullOrWhiteSpace(createDto.Slug)
                ? GenerateSlug(createDto.Title)
                : GenerateSlug(createDto.Slug);

            slug = await EnsureUniqueSlugAsync(slug);

            var blog = new Blog
            {
                Title = createDto.Title,
                Slug = slug,
                Excerpt = createDto.Excerpt ?? GenerateExcerpt(createDto.Content),
                Content = createDto.Content,
                CoverImage = createDto.CoverImage,
                CoverImageAlt = createDto.CoverImageAlt ?? createDto.Title,
                BannerImage = createDto.BannerImage,
                AuthorName = createDto.AuthorName ?? "Adotzee Editorial Team",
                AuthorRole = createDto.AuthorRole ?? "Education Specialist",
                AuthorAvatar = createDto.AuthorAvatar,
                Category = createDto.Category ?? "General",
                Tags = createDto.Tags,
                Status = createDto.Status,
                Featured = createDto.Featured,
                ReadTimeMinutes = createDto.ReadTimeMinutes > 0 ? createDto.ReadTimeMinutes : EstimateReadTime(createDto.Content),
                SeoTitle = createDto.SeoTitle ?? createDto.Title,
                MetaDescription = createDto.MetaDescription ?? createDto.Excerpt,
                CanonicalUrl = createDto.CanonicalUrl,
                PublishedAt = createDto.Status == BlogStatus.Published ? (createDto.PublishedAt ?? DateTime.UtcNow) : createDto.PublishedAt,
                ScheduledAt = createDto.ScheduledAt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return MapToDto(blog);
        }

        public async Task<BlogResponseDto?> UpdateAsync(int id, UpdateBlogDto updateDto)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return null;

            if (!string.IsNullOrWhiteSpace(updateDto.Slug) && updateDto.Slug.ToLower() != blog.Slug.ToLower())
            {
                var newSlug = GenerateSlug(updateDto.Slug);
                blog.Slug = await EnsureUniqueSlugAsync(newSlug, id);
            }

            blog.Title = updateDto.Title;
            blog.Excerpt = updateDto.Excerpt ?? GenerateExcerpt(updateDto.Content);
            blog.Content = updateDto.Content;
            blog.CoverImage = updateDto.CoverImage;
            blog.CoverImageAlt = updateDto.CoverImageAlt ?? updateDto.Title;
            blog.BannerImage = updateDto.BannerImage;
            blog.AuthorName = updateDto.AuthorName;
            blog.AuthorRole = updateDto.AuthorRole;
            blog.AuthorAvatar = updateDto.AuthorAvatar;
            blog.Category = updateDto.Category;
            blog.Tags = updateDto.Tags;
            blog.Featured = updateDto.Featured;
            blog.ReadTimeMinutes = updateDto.ReadTimeMinutes > 0 ? updateDto.ReadTimeMinutes : EstimateReadTime(updateDto.Content);
            blog.SeoTitle = updateDto.SeoTitle;
            blog.MetaDescription = updateDto.MetaDescription;
            blog.CanonicalUrl = updateDto.CanonicalUrl;
            blog.ScheduledAt = updateDto.ScheduledAt;

            // Handle status transitions
            if (blog.Status != updateDto.Status)
            {
                blog.Status = updateDto.Status;
                if (updateDto.Status == BlogStatus.Published && !blog.PublishedAt.HasValue)
                {
                    blog.PublishedAt = DateTime.UtcNow;
                }
            }

            blog.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToDto(blog);
        }

        public async Task<BlogResponseDto?> UpdateStatusAsync(int id, BlogStatus status)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return null;

            blog.Status = status;
            if (status == BlogStatus.Published && !blog.PublishedAt.HasValue)
            {
                blog.PublishedAt = DateTime.UtcNow;
            }

            blog.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToDto(blog);
        }

        public async Task<BlogResponseDto?> ToggleFeatureAsync(int id, bool featured)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return null;

            blog.Featured = featured;
            blog.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToDto(blog);
        }

        public async Task<bool> IncrementViewsAsync(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return false;

            blog.ViewsCount += 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncrementLikesAsync(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return false;

            blog.LikesCount += 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return false;

            blog.IsDeleted = true;
            blog.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        private static BlogResponseDto MapToDto(Blog blog)
        {
            return new BlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Slug = blog.Slug,
                Excerpt = blog.Excerpt,
                Content = blog.Content,
                CoverImage = blog.CoverImage,
                CoverImageAlt = blog.CoverImageAlt,
                BannerImage = blog.BannerImage,
                AuthorName = blog.AuthorName,
                AuthorRole = blog.AuthorRole,
                AuthorAvatar = blog.AuthorAvatar,
                Category = blog.Category,
                Tags = blog.Tags,
                Status = blog.Status,
                Featured = blog.Featured,
                ReadTimeMinutes = blog.ReadTimeMinutes,
                ViewsCount = blog.ViewsCount,
                LikesCount = blog.LikesCount,
                SharesCount = blog.SharesCount,
                SeoTitle = blog.SeoTitle,
                MetaDescription = blog.MetaDescription,
                CanonicalUrl = blog.CanonicalUrl,
                PublishedAt = blog.PublishedAt,
                ScheduledAt = blog.ScheduledAt,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
        }

        private static string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "post";
            string str = text.ToLowerInvariant();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        private async Task<string> EnsureUniqueSlugAsync(string baseSlug, int currentId = 0)
        {
            var slug = baseSlug;
            var counter = 1;

            while (await _context.Blogs.AnyAsync(b => b.Slug == slug && b.Id != currentId))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        private static string GenerateExcerpt(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return "";
            var plainText = Regex.Replace(content, @"<[^>]+>|#|\*|_", "").Trim();
            return plainText.Length > 250 ? plainText.Substring(0, 247) + "..." : plainText;
        }

        private static int EstimateReadTime(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return 3;
            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return Math.Max(1, (int)Math.Ceiling(words / 200.0));
        }
    }
}
