using Adotzee_Backend.DTOs.ScholarshipDTOs;
using Adotzee_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adotzee_Backend.Services.ScholarshipServices
{
    public interface IScholarshipService
    {
        Task<ApiResponse<IEnumerable<ScholarshipDTO>>> GetActiveScholarshipsAsync();
        Task<ApiResponse<string>> ApplyForScholarshipAsync(CreateScholarshipEnquiryDTO applicationDto);
    }
}
