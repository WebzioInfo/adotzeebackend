using Adotzee_Backend.DTOs.SearchDTOs;
using Adotzee_Backend.Models;

namespace Adotzee_Backend.Services.SearchServices
{
    public interface ISearchService
    {
        Task<ApiResponse<GlobalSearchResponseDTO>> GlobalSearchAsync(string query);
    }
}