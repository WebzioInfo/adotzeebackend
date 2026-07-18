using Adotzee_Backend.DTOs.SearchDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.SearchRepos;

namespace Adotzee_Backend.Services.SearchServices
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _repo;

        public SearchService(ISearchRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<GlobalSearchResponseDTO>> GlobalSearchAsync(string query)
        {
            var (courses, colleges, addons) = await _repo.GlobalSearchAsync(query);

            var responseDto = new GlobalSearchResponseDTO
            {
                Courses = courses,
                Colleges = colleges,
                Addons = addons
            };

            return ApiResponse<GlobalSearchResponseDTO>.SuccessResponse(responseDto, "Search completed successfully");
        }
    }
}