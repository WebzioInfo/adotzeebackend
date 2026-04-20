using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.LeadServices
{
    public interface ILeadService
    {
        Task<ApiResponse<object>> GetAllAsync(int? cursor = null, int pageSize = 10, string? search = null, string? source = null, string? status = null);
        Task<ApiResponse<LeadResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(LeadCreateDTO dto);
        Task<ApiResponse<string>> UpdateAsync(LeadUpdateDTO dto);
        Task<ApiResponse<string>> UpdateStatusAsync(int id, LeadStatus status);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<LeadDashboardStatsDTO>> GetDashboardStatsAsync();
    }
}
