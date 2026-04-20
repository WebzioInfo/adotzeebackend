using Adotzee_Backend.Data;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.AddonRepos
{
    public class AddonRepository : IAddonRepository
    {
        private readonly AppDbContext _context;

        public AddonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddonCourse>> GetAllAsync()
        {
            return await _context.AddonCourses
                .Include(a => a.Course)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .OrderBy(a => a.DisplayOrder)
                .ToListAsync();
        }

        public async Task<PagedResponse<AddonCourse>> GetPagedAsync(PaginationParams @params)
        {
            var query = _context.AddonCourses
                .Include(a => a.Course)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(@params.Search))
            {
                var lowerSearch = @params.Search.ToLower();
                query = query.Where(a => a.Name.ToLower().Contains(lowerSearch) ||
                                         a.Course.Name.ToLower().Contains(lowerSearch));
            }

            int totalCount = await query.CountAsync();
            var items = await query.OrderBy(a => a.DisplayOrder)
                                   .Skip((@params.PageNumber - 1) * @params.PageSize)
                                   .Take(@params.PageSize)
                                   .ToListAsync();

            return new PagedResponse<AddonCourse>(items, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task UpdateOrderAsync(List<int> ids)
        {
            var addons = await _context.AddonCourses.Where(a => ids.Contains(a.Id)).ToListAsync();
            for (int i = 0; i < ids.Count; i++)
            {
                var addon = addons.FirstOrDefault(a => a.Id == ids[i]);
                if (addon != null)
                {
                    addon.DisplayOrder = i;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<AddonCourse?> GetByIdAsync(int id)
        {
            return await _context.AddonCourses
                .Include(a => a.Course)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AddonCourse> CreateAsync(AddonCourse addon)
        {
            _context.AddonCourses.Add(addon);
            await _context.SaveChangesAsync();
            return addon;
        }

        public async Task<AddonCourse> UpdateAsync(AddonCourse addon)
        {
            _context.AddonCourses.Update(addon);
            await _context.SaveChangesAsync();
            return addon;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var addon = await _context.AddonCourses
                .Include(a => a.AddonColleges)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (addon == null) return false;

            _context.AddonCourses.Remove(addon);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AddonCourse>> GetByCourseIdAsync(int courseId)
        {
            return await _context.AddonCourses
                .Where(a => a.CourseId == courseId)
                .Include(a => a.Course)
                .Include(a => a.AddonColleges).ThenInclude(ac => ac.College)
                .ToListAsync();
        }
        public async Task<IEnumerable<College>> GetCollegesByAddonIdAsync(int addonCourseId)
        {
            return await _context.AddonColleges
                .Where(ac => ac.AddonCourseId == addonCourseId)
                .Include(ac => ac.College)
                .Select(ac => ac.College)
                .ToListAsync();
        }

    }
}
