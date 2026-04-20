using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.SearchRepos
{
    public interface ISearchRepository
    {
        Task<(IEnumerable<Course> Courses, IEnumerable<College> Colleges, IEnumerable<AddonCourse> Addons)> GlobalSearchAsync(string query);
    }
}
