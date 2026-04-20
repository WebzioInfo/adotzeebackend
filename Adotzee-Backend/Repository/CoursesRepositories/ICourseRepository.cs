using Adotzee_Backend.Helpers;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.CoursesRepositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course> AddAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(Course course);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<List<Course>> FilterByTypeStreamAsync(CourseType type, StreamType stream);
        Task<IEnumerable<AddonCourse>> GetAddonCoursesByCourseIdAsync(int courseId);
        Task<PagedResponse<Course>> GetPagedAsync(PaginationParams @params);
        Task UpdateOrderAsync(List<int> ids);
    }
}
