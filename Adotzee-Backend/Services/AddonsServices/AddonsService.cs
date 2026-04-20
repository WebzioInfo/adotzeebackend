using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.AddonRepos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Services.AddonsServices
{
    public class AddonsService : IAddonsService
    {
        private readonly IAddonRepository _repo;
        private readonly IMapper _mapper;

        public AddonsService(IAddonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AddonCourseResponseDTO>>> GetAllAsync()
        {
            try
            {
                var data = await _repo.GetAllAsync();
                var result = _mapper.Map<List<AddonCourseResponseDTO>>(data);
                return ApiResponse<List<AddonCourseResponseDTO>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AddonCourseResponseDTO>>.FailResponse("Error fetching addons: " + ex.Message);
            }
        }

        public async Task<ApiResponse<PagedResponse<AddonCourseResponseDTO>>> GetPagedAsync(PaginationParams @params)
        {
            try
            {
                var paged = await _repo.GetPagedAsync(@params);
                var mappedItems = _mapper.Map<List<AddonCourseResponseDTO>>(paged.Items);
                var response = new PagedResponse<AddonCourseResponseDTO>(mappedItems, paged.TotalCount, paged.PageNumber, paged.PageSize);
                return ApiResponse<PagedResponse<AddonCourseResponseDTO>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<AddonCourseResponseDTO>>.FailResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<string>> ReorderAsync(List<int> ids)
        {
            try
            {
                await _repo.UpdateOrderAsync(ids);
                return ApiResponse<string>.SuccessResponse("Order updated");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Error updating order: " + ex.Message);
            }
        }

        public async Task<ApiResponse<AddonCourseResponseDTO>> GetByIdAsync(int id)
        {
            try
            {
                var addon = await _repo.GetByIdAsync(id);
                if (addon == null)
                    return ApiResponse<AddonCourseResponseDTO>.FailResponse("Addon not found");

                var result = _mapper.Map<AddonCourseResponseDTO>(addon);
                return ApiResponse<AddonCourseResponseDTO>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<AddonCourseResponseDTO>.FailResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<AddonCourseResponseDTO>> CreateAsync(AddonCourseCreateDTO dto)
        {
            try
            {
                var addon = _mapper.Map<AddonCourse>(dto);
                var saved = await _repo.CreateAsync(addon);
                var result = _mapper.Map<AddonCourseResponseDTO>(saved);
                return ApiResponse<AddonCourseResponseDTO>.SuccessResponse(result, "Addon created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AddonCourseResponseDTO>.FailResponse("Error creating addon: " + ex.Message);
            }
        }

        public async Task<ApiResponse<AddonCourseResponseDTO>> UpdateAsync(AddonCourseUpdateDTO dto)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(dto.Id);
                if (existing == null)
                    return ApiResponse<AddonCourseResponseDTO>.FailResponse("Addon not found");

                // Clear existing colleges
                existing.AddonColleges.Clear();

                // Map updated values
                _mapper.Map(dto, existing);
                existing.AddonColleges = dto.CollegeIds.Select(id => new AddonCollege { CollegeId = id }).ToList();

                var updated = await _repo.UpdateAsync(existing);
                var result = _mapper.Map<AddonCourseResponseDTO>(updated);

                return ApiResponse<AddonCourseResponseDTO>.SuccessResponse(result, "Addon updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AddonCourseResponseDTO>.FailResponse("Error updating addon: " + ex.Message);
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            try
            {
                var success = await _repo.DeleteAsync(id);
                if (!success)
                    return ApiResponse<string>.FailResponse("Addon not found");

                return ApiResponse<string>.SuccessResponse("Addon deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Error deleting addon: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<AddonCourseResponseDTO>>> GetByCourseIdAsync(int courseId)
        {
            try
            {
                var addons = await _repo.GetByCourseIdAsync(courseId);
                var result = _mapper.Map<List<AddonCourseResponseDTO>>(addons);
                return ApiResponse<List<AddonCourseResponseDTO>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AddonCourseResponseDTO>>.FailResponse("Error fetching by course: " + ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<CollegeResponseDTO>>> GetCollegesByAddonIdAsync(int addonCourseId)
        {
            try
            {
                var colleges = await _repo.GetCollegesByAddonIdAsync(addonCourseId);

                if (colleges == null || !colleges.Any())
                {
                    return ApiResponse<IEnumerable<CollegeResponseDTO>>.FailResponse("No colleges found for the specified Addon Course.");
                }

                var responseDTOs = _mapper.Map<IEnumerable<CollegeResponseDTO>>(colleges);

                return ApiResponse<IEnumerable<CollegeResponseDTO>>.SuccessResponse(responseDTOs, "Colleges retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<CollegeResponseDTO>>.FailResponse($"An error occurred: {ex.Message}");
            }
        }

    }
}
