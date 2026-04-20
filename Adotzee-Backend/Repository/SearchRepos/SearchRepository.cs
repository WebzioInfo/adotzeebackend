using Adotzee_Backend.Data;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.SearchRepos
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AppDbContext _context;

        public SearchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Course> Courses, IEnumerable<College> Colleges, IEnumerable<AddonCourse> Addons)> GlobalSearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return (Enumerable.Empty<Course>(), Enumerable.Empty<College>(), Enumerable.Empty<AddonCourse>());
            }

            query = query.ToLower();

            // Search Courses (Name, Type, Stream)
            var courses = await _context.Courses
                .AsNoTracking()
                .Where(c => c.Name.ToLower().Contains(query))
                // Note: CourseType and StreamType are Enums, so we search dynamically or compare against names if needed.
                // Assuming Name is the primary text field for Course. To handle Enum string matching we evaluate them in memory if required,
                // but usually, text searching is done primarily against string fields like Name in DB.
                // It is better to use string properties if available in the database to avoid translation errors.
                // Let's stick with string mapping where possible. For simplicity, filtering by name dynamically.
                .Take(10)
                .ToListAsync();

            // Search Colleges (Name, Address)
            var colleges = await _context.Colleges
                .AsNoTracking()
                .Where(c => (c.Name != null && c.Name.ToLower().Contains(query)) ||
                            (c.Address != null && c.Address.ToLower().Contains(query)))
                .Take(10)
                .ToListAsync();

            // Search Addons (Name)
            var addons = await _context.AddonCourses
                .AsNoTracking()
                .Where(a => a.Name.ToLower().Contains(query))
                .Take(10)
                .ToListAsync();

            return (courses, colleges, addons);
        }
    }
}
