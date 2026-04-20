using Adotzee_Backend.DTOs;
using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.AddonsServices
{
    public interface IAddonsService
    {
        Task<ApiResponse<List<AddonCourseResponseDTO>>> GetAllAsync();
        Task<ApiResponse<AddonCourseResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<AddonCourseResponseDTO>> CreateAsync(AddonCourseCreateDTO dto);
        Task<ApiResponse<AddonCourseResponseDTO>> UpdateAsync(AddonCourseUpdateDTO dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<List<AddonCourseResponseDTO>>> GetByCourseIdAsync(int courseId);
        Task<ApiResponse<IEnumerable<CollegeResponseDTO>>> GetCollegesByAddonIdAsync(int addonCourseId);
        Task<ApiResponse<PagedResponse<AddonCourseResponseDTO>>> GetPagedAsync(PaginationParams @params);
        Task<ApiResponse<string>> ReorderAsync(List<int> ids);

    }

}
