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
            var courses = await _repo.GetAllAsync();
            var response = _mapper.Map<List<CourseResponseDTO>>(courses);
            return ApiResponse<List<CourseResponseDTO>>.SuccessResponse(response);
        }

        public async Task<ApiResponse<PagedResponse<CourseResponseDTO>>> GetPagedAsync(PaginationParams @params)
        {
            var paged = await _repo.GetPagedAsync(@params);
            var mappedItems = _mapper.Map<List<CourseResponseDTO>>(paged.Items);
            var response = new PagedResponse<CourseResponseDTO>(mappedItems, paged.TotalCount, paged.PageNumber, paged.PageSize);
            return ApiResponse<PagedResponse<CourseResponseDTO>>.SuccessResponse(response);
        }

        public async Task<ApiResponse<string>> ReorderAsync(List<int> ids)
        {
            await _repo.UpdateOrderAsync(ids);
            return ApiResponse<string>.SuccessResponse("Order updated");
        }

        public async Task<ApiResponse<CourseResponseDTO>> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return ApiResponse<CourseResponseDTO>.FailResponse("Course not found");

            return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course));
        }

        public async Task<ApiResponse<CourseResponseDTO>> CreateAsync(CourseCreateDTO dto)
        {
            var course = _mapper.Map<Course>(dto);
            await _repo.AddAsync(course);
            return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course), "Course created");
        }

        public async Task<ApiResponse<CourseResponseDTO>> UpdateAsync(CourseUpdateDTO dto)
        {
            var course = await _repo.GetByIdAsync(dto.Id);
            if (course == null) return ApiResponse<CourseResponseDTO>.FailResponse("Course not found");

            _mapper.Map(dto, course);
            await _repo.UpdateAsync(course);

            return ApiResponse<CourseResponseDTO>.SuccessResponse(_mapper.Map<CourseResponseDTO>(course), "Course updated");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return ApiResponse<string>.FailResponse("Course not found");

            await _repo.DeleteAsync(course);
            return ApiResponse<string>.SuccessResponse("Course deleted");
        }
        public async Task<ApiResponse<object>> GetDashboardStats()
        {
            var totalCourses = await _repo.GetTotalCountAsync();
            return ApiResponse<object>.SuccessResponse(new
            {
                TotalCourses = totalCourses
            });
        }

        public async Task<ApiResponse<List<CourseResponseDTO>>> FilterByTypeStreamAsync(string? type, string? stream)
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
        public async Task<ApiResponse<IEnumerable<AddonCourseResponseDTO>>> GetAddonCoursesByCourseIdAsync(int courseId)
        {
            var addons = await _repo.GetAddonCoursesByCourseIdAsync(courseId);

            if (addons == null || !addons.Any())
            {
                return ApiResponse<IEnumerable<AddonCourseResponseDTO>>.FailResponse("No addon courses found for the specified Course.");
            }

            var responseDTOs = _mapper.Map<IEnumerable<AddonCourseResponseDTO>>(addons);

            return ApiResponse<IEnumerable<AddonCourseResponseDTO>>.SuccessResponse(responseDTOs, "Addon courses retrieved successfully.");
        }


    }
}