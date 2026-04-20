using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.CollegeServices
{
    public interface ICollegeService
    {
        Task<ApiResponse<List<CollegeResponseDTO>>> GetAllAsync();
        Task<ApiResponse<CollegeResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(CollegeCreateDTO dto);
        Task<ApiResponse<string>> UpdateAsync(CollegeUpdateDTO dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<PagedResponse<CollegeResponseDTO>>> GetPagedAsync(PaginationParams @params);
        Task<ApiResponse<string>> ReorderAsync(List<int> ids);
    }
}
