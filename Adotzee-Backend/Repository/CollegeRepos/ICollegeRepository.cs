using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.CollegeRepos
{
    public interface ICollegeRepository
    {
        Task<List<College>> GetAllAsync();
        Task<College?> GetByIdAsync(int id);
        Task AddAsync(College college, List<int>? addonIds);
        Task UpdateAsync(College college, List<int>? addonIds);
        Task DeleteAsync(College college);
        Task<PagedResponse<College>> GetPagedAsync(PaginationParams @params);
        Task UpdateOrderAsync(List<int> ids);
    }
}
