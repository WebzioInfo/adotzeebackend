using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.AddonRepos
{
    public interface IAddonRepository
    {
        Task<List<AddonCourse>> GetAllAsync();
        Task<AddonCourse?> GetByIdAsync(int id);
        Task<AddonCourse> CreateAsync(AddonCourse addon);
        Task<AddonCourse> UpdateAsync(AddonCourse addon);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AddonCourse>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<College>> GetCollegesByAddonIdAsync(int addonCourseId);

        Task<PagedResponse<AddonCourse>> GetPagedAsync(PaginationParams @params);
        Task UpdateOrderAsync(List<int> ids);
    }
}
