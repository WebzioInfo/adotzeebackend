using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.LeadRepos
{
    public interface ILeadRepository
    {
        Task<(List<Lead> Leads, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? search = null, string? source = null, string? status = null);
        
        // Cursor-based pagination
        Task<(List<Lead> Leads, bool HasMore, int? NextCursor)> GetAllPagedAsync(int? cursor = null, int pageSize = 10, string? search = null, string? source = null, string? status = null);

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
