using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs.Review;
using Adotzee_Backend.Models;
using Adotzee_Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.ReviewRepos
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Review>> GetAllAsync(ReviewQueryParametersDto queryParameters)
        {
            var query = _context.Reviews.AsQueryable();

            if (queryParameters.Status.HasValue)
            {
                query = query.Where(r => r.Status == queryParameters.Status.Value);
            }

            if (queryParameters.Featured.HasValue)
            {
                query = query.Where(r => r.Featured == queryParameters.Featured.Value);
            }

            if (queryParameters.Rating.HasValue)
            {
                query = query.Where(r => r.Rating == queryParameters.Rating.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Course))
            {
                query = query.Where(r => r.Course.Contains(queryParameters.Course));
            }

            if (!string.IsNullOrEmpty(queryParameters.CollegeName))
            {
                query = query.Where(r => r.CollegeName != null && r.CollegeName.Contains(queryParameters.CollegeName));
            }

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                query = query.Where(r => 
                    r.FullName.Contains(queryParameters.Search) || 
                    r.Email.Contains(queryParameters.Search) || 
                    (r.CollegeName != null && r.CollegeName.Contains(queryParameters.Search)) || 
                    r.Course.Contains(queryParameters.Search));
            }

            // Always order by latest first
            query = query.OrderByDescending(r => r.CreatedAt);

            var totalCount = await query.CountAsync();

            int pageNumber = queryParameters.PageNumber ?? 1;
            int pageSize = queryParameters.PageSize ?? 10;
            pageSize = pageSize > 100 ? 100 : pageSize; // cap at 100

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Review>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<List<Review>> GetFeaturedAsync()
        {
            var featured = await _context.Reviews
                .Where(r => r.Featured && r.Status == ReviewStatus.Approved)
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .ToListAsync();

            if (featured.Count == 0)
            {
                return await _context.Reviews
                    .Where(r => r.Status == ReviewStatus.Approved)
                    .OrderByDescending(r => r.Rating)
                    .ThenByDescending(r => r.CreatedAt)
                    .Take(10)
                    .ToListAsync();
            }

            return featured;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Review review)
        {
            review.IsDeleted = true;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
    }
}
