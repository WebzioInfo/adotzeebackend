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
            return await _context.Courses.AsNoTracking().OrderBy(c => c.DisplayOrder).ToListAsync();
        }

        public async Task<PagedResponse<Course>> GetPagedAsync(PaginationParams @params)
        {
            var query = _context.Courses.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(@params.Search))
            {
                var lowerSearch = @params.Search.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(lowerSearch) ||
                                         c.Type.ToString().ToLower().Contains(lowerSearch) ||
                                         c.Stream.ToString().ToLower().Contains(lowerSearch));
            }

            int totalCount = await query.CountAsync();
            var items = await query.OrderBy(c => c.DisplayOrder)
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
                    course.DisplayOrder = i;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> AddAsync(Course course)
        {
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

        public async Task<List<Course>> FilterByTypeStreamAsync(CourseType? type, StreamType? stream)
        {
            var query = _context.Courses.AsNoTracking().AsQueryable();

            if (type.HasValue)
                query = query.Where(c => c.Type == type.Value);

            if (stream.HasValue)
                query = query.Where(c => c.Stream == stream.Value);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<AddonCourse>> GetAddonCoursesByCourseIdAsync(int courseId)
        {
            return await _context.AddonCourses
                .Where(a => a.CourseId == courseId)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
