using Adotzee_Backend.Data;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.CollegeRepos
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly AppDbContext _context;

        public CollegeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<College>> GetAllAsync()
        {
            return await _context.Colleges
                .Include(c => c.AddonColleges)
                    .ThenInclude(ac => ac.AddonCourse)
                .AsNoTracking()
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<PagedResponse<College>> GetPagedAsync(PaginationParams @params)
        {
            var query = _context.Colleges
                .Include(c => c.AddonColleges)
                    .ThenInclude(ac => ac.AddonCourse)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(@params.Search))
            {
                var lowerSearch = @params.Search.ToLower();
                query = query.Where(c => (c.Name != null && c.Name.ToLower().Contains(lowerSearch)) ||
                                         (c.Address != null && c.Address.ToLower().Contains(lowerSearch)));
            }

            int totalCount = await query.CountAsync();
            var items = await query.OrderBy(c => c.DisplayOrder)
                                   .Skip((@params.PageNumber - 1) * @params.PageSize)
                                   .Take(@params.PageSize)
                                   .ToListAsync();

            return new PagedResponse<College>(items, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task UpdateOrderAsync(List<int> ids)
        {
            var colleges = await _context.Colleges.Where(c => ids.Contains(c.Id)).ToListAsync();
            for (int i = 0; i < ids.Count; i++)
            {
                var college = colleges.FirstOrDefault(c => c.Id == ids[i]);
                if (college != null)
                {
                    college.DisplayOrder = i;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<College?> GetByIdAsync(int id)
        {
            return await _context.Colleges
                .Include(c => c.AddonColleges)
                    .ThenInclude(ac => ac.AddonCourse)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(College college, List<int>? addonIds)
        {
            _context.Colleges.Add(college);
            await _context.SaveChangesAsync();

            if (addonIds != null && addonIds.Any())
            {
                foreach (var addonId in addonIds)
                {
                    _context.AddonColleges.Add(new AddonCollege
                    {
                        CollegeId = college.Id,
                        AddonCourseId = addonId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(College college, List<int>? addonIds)
        {
            _context.Colleges.Update(college);
            await _context.SaveChangesAsync();

            var existing = _context.AddonColleges.Where(ac => ac.CollegeId == college.Id);
            _context.AddonColleges.RemoveRange(existing);
            await _context.SaveChangesAsync();

            if (addonIds != null && addonIds.Any())
            {
                foreach (var addonId in addonIds)
                {
                    _context.AddonColleges.Add(new AddonCollege
                    {
                        CollegeId = college.Id,
                        AddonCourseId = addonId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(College college)
        {
            var related = _context.AddonColleges.Where(ac => ac.CollegeId == college.Id);
            _context.AddonColleges.RemoveRange(related);

            _context.Colleges.Remove(college);
            await _context.SaveChangesAsync();
        }
    }
}
