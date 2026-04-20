using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs;
using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.DTOs.CourseDTOs;
using Adotzee_Backend.Helpers;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository;
using Adotzee_Backend.Repository.CoursesRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CourseResponseDTO>>> GetAllAsync()
        {
            try
            {
                var courses = await _repo.GetAllAsync();
                var response = _mapper.Map<List<CourseResponseDTO>>(courses);
                return ApiResponse<List<CourseResponseDTO>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CourseResponseDTO>>.FailResponse("Error fetching courses: " + ex.Message);
            }
        }

        public async Task<ApiResponse<PagedResponse<CourseResponseDTO>>> GetPagedAsync(PaginationParams @params)
        {
            try
            {
                var paged = await _repo.GetPagedAsync(@params);
                var mappedItems = _mapper.Map<List<CourseResponseDTO>>(paged.Items);
                var response = new PagedResponse<CourseResponseDTO>(mappedItems, paged.TotalCount, paged.PageNumber, paged.PageSize);
                return ApiResponse<PagedResponse<CourseResponseDTO>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CourseResponseDTO>>.FailResponse("Error: " + ex.Message);
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

        public async Task<ApiResponse<CourseResponseDTO>> GetByIdAsync(int id)
        {
            try
            {
                var course = await _repo.GetByIdAsync(id);
                if (course == null) return ApiResponse<CourseResponseDTO>.FailResponse("Course not found");

                return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course));
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponseDTO>.FailResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<CourseResponseDTO>> CreateAsync(CourseCreateDTO dto)
        {
            try
            {
                var course = _mapper.Map<Course>(dto);
                await _repo.AddAsync(course);
                return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course), "Course created");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponseDTO>.FailResponse("Error creating course: " + ex.Message);
            }
        }

        public async Task<ApiResponse<CourseResponseDTO>> UpdateAsync(CourseUpdateDTO dto)
        {
            try
            {
                var course = await _repo.GetByIdAsync(dto.Id);
                if (course == null) return ApiResponse<CourseResponseDTO>.FailResponse("Course not found");

                _mapper.Map(dto, course);
                await _repo.UpdateAsync(course);

                return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course), "Course updated");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponseDTO>.FailResponse("Error updating course: " + ex.Message);
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            try
            {
                var course = await _repo.GetByIdAsync(id);
                if (course == null) return ApiResponse<string>.FailResponse("Course not found");

                await _repo.DeleteAsync(course);
                return ApiResponse<string>.SuccessResponse("Course deleted");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Error deleting course: " + ex.Message);
            }
        }
        public async Task<ApiResponse<object>> GetDashboardStats()
        {
            try
            {
                var totalCourses = await _repo.GetTotalCountAsync();
                return ApiResponse<object>.SuccessResponse(new
                {
                    TotalCourses = totalCourses
                });
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse("Failed to get stats: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<CourseResponseDTO>>> FilterByTypeStreamAsync(string? type, string? stream)
        {
            try
            {
                CourseType? courseType = null;
                StreamType? streamType = null;

                // Parse CourseType if provided
                if (!string.IsNullOrEmpty(type))
                {
                    if (!Enum.TryParse<CourseType>(type, true, out var parsedType))
                        return ApiResponse<List<CourseResponseDTO>>.FailResponse("Invalid course type");

                    courseType = parsedType;
                }

                // Parse StreamType if provided
                if (!string.IsNullOrEmpty(stream))
                {
                    if (!Enum.TryParse<StreamType>(stream, true, out var parsedStream))
                        return ApiResponse<List<CourseResponseDTO>>.FailResponse("Invalid stream type");

                    streamType = parsedStream;
                }

                var result = await _repo.FilterByTypeStreamAsync(courseType, streamType);
                var mapped = _mapper.Map<List<CourseResponseDTO>>(result);

                return ApiResponse<List<CourseResponseDTO>>.SuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CourseResponseDTO>>.FailResponse("Error filtering: " + ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<AddonCourseResponseDTO>>> GetAddonCoursesByCourseIdAsync(int courseId)
        {
            try
            {
                var addons = await _repo.GetAddonCoursesByCourseIdAsync(courseId);

                if (addons == null || !addons.Any())
                {
                    return ApiResponse<IEnumerable<AddonCourseResponseDTO>>.FailResponse("No addon courses found for the specified Course.");
                }

                var responseDTOs = _mapper.Map<IEnumerable<AddonCourseResponseDTO>>(addons);

                return ApiResponse<IEnumerable<AddonCourseResponseDTO>>.SuccessResponse(responseDTOs, "Addon courses retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<AddonCourseResponseDTO>>.FailResponse($"An error occurred: {ex.Message}");
            }
        }


    }
}
