using Adotzee_Backend.DTOs;
using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.DTOs.CourseDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.CourseServices
{
    public interface ICourseService
    {
        Task<ApiResponse<List<CourseResponseDTO>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<CourseResponseDTO>>> GetPagedAsync(PaginationParams @params);
        Task<ApiResponse<string>> ReorderAsync(List<int> ids);
        Task<ApiResponse<CourseResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<CourseResponseDTO>> CreateAsync(CourseCreateDTO dto);
        Task<ApiResponse<CourseResponseDTO>> UpdateAsync(CourseUpdateDTO dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<object>> GetDashboardStats();
        Task<ApiResponse<List<CourseResponseDTO>>> FilterByTypeStreamAsync(string type, string stream);
        Task<ApiResponse<IEnumerable<AddonCourseResponseDTO>>> GetAddonCoursesByCourseIdAsync(int courseId);
    }
}