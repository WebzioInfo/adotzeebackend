using Adotzee_Backend.Data;
using Adotzee_Backend.Helpers;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.CoursesRepositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.OrderBy(c => c.DisplayOrder).ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> AddAsync(Course course)
        {
            var maxOrder = await _context.Courses.MaxAsync(c => (int?)c.DisplayOrder) ?? 0;
            course.DisplayOrder = maxOrder + 1;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<List<Course>> FilterByTypeStreamAsync(CourseType type, StreamType stream)
        {
            return await _context.Courses
                .Where(c => c.Type == type && c.Stream == stream)
                .ToListAsync();
        }
        public async Task<IEnumerable<AddonCourse>> GetAddonCoursesByCourseIdAsync(int courseId)
        {
            return await _context.AddonCourses
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.DisplayOrder)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .ToListAsync();
        }

        public async Task<PagedResponse<Course>> GetPagedAsync(PaginationParams @params)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(@params.Search))
            {
                query = query.Where(c => c.Name.Contains(@params.Search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.DisplayOrder)
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToListAsync();

            return new PagedResponse<Course>(items, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task UpdateOrderAsync(List<int> ids)
        {
            var courses = await _context.Courses.Where(c => ids.Contains(c.Id)).ToListAsync();
            for (int i = 0; i < ids.Count; i++)
            {
                var course = courses.FirstOrDefault(c => c.Id == ids[i]);
                if (course != null)
                {
                    course.DisplayOrder = i + 1;
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
