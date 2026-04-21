using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.LeadRepos
{
    public interface ILeadRepository
    {
        Task<PagedResponse<Lead>> GetPagedAsync(PaginationParams @params);

        Task<Lead?> GetByIdAsync(int id);
        Task<Lead> AddAsync(Lead lead);
        Task<Lead> UpdateAsync(Lead lead);
        Task<Lead> DeleteAsync(Lead lead);
        
        // Optimized Dashboard Stats
        Task<LeadDashboardStatsDTO> GetDashboardStatsAsync();

        // Legacy individual stats methods (consider removing after transition)
        Task<int> GetTotalLeadsAsync();
        Task<int> GetTotalNewLeadsAsync();
        Task<int> GetTotalContactedLeadsAsync();
        Task<int> GetTotalConvertedLeadsAsync();
        Task<int> GetTotalRejectedLeadsAsync();
        Task<double> GetConversionRateAsync();
        Task<int> GetLeadsTodayAsync();
        Task<int> GetLeadsThisMonthAsync();
        Task<Dictionary<string, int>> GetLeadsBySourceAsync();
        Task<Dictionary<string, int>> GetLeadsByStatusAsync();
        Task<Dictionary<string, int>> GetLeadsByPriorityAsync();
        Task<List<MonthWiseDTO>> GetMonthlyTrendAsync();
    }
}
