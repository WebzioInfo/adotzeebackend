using Adotzee_Backend.DTOs.RecommendationDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.RecommendationRepos
{
    public interface IRecommendationRepository
    {
        Task<(IEnumerable<Course> Courses, IEnumerable<College> Colleges, IEnumerable<AddonCourse> Addons)> GetRecommendationsAsync(RecommendationRequestDTO request);
    }
}
