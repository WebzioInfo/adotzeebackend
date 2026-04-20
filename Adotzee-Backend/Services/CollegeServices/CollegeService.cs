using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.CollegeRepos;
using Adotzee_Backend.Services.CollegeServices;
using AutoMapper;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _repo;
    private readonly IMapper _mapper;

    public CollegeService(ICollegeRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CollegeResponseDTO>>> GetAllAsync()
    {
        var colleges = await _repo.GetAllAsync();
        var dto = _mapper.Map<List<CollegeResponseDTO>>(colleges);
        return ApiResponse<List<CollegeResponseDTO>>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResponse<CollegeResponseDTO>>> GetPagedAsync(PaginationParams @params)
    {
        try
        {
            var paged = await _repo.GetPagedAsync(@params);
            var mappedItems = _mapper.Map<List<CollegeResponseDTO>>(paged.Items);
            var response = new PagedResponse<CollegeResponseDTO>(mappedItems, paged.TotalCount, paged.PageNumber, paged.PageSize);
            return ApiResponse<PagedResponse<CollegeResponseDTO>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<PagedResponse<CollegeResponseDTO>>.FailResponse("Error: " + ex.Message);
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

    public async Task<ApiResponse<CollegeResponseDTO>> GetByIdAsync(int id)
    {
        var college = await _repo.GetByIdAsync(id);
        if (college == null)
            return ApiResponse<CollegeResponseDTO>.FailResponse("College not found");

        var dto = _mapper.Map<CollegeResponseDTO>(college);
        return ApiResponse<CollegeResponseDTO>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<string>> CreateAsync(CollegeCreateDTO dto)
    {
        if (!IsValidLocation(dto.Latitude, dto.Longitude))
            return ApiResponse<string>.FailResponse("Invalid latitude or longitude");

        var college = _mapper.Map<College>(dto);
        await _repo.AddAsync(college, dto.AddonIds);
        return ApiResponse<string>.SuccessResponse("College created successfully");
    }


    public async Task<ApiResponse<string>> UpdateAsync(CollegeUpdateDTO dto)
    {
        var college = await _repo.GetByIdAsync(dto.Id);
        if (college == null)
            return ApiResponse<string>.FailResponse("College not found");

        if (!IsValidLocation(dto.Latitude, dto.Longitude))
            return ApiResponse<string>.FailResponse("Invalid latitude or longitude");

        _mapper.Map(dto, college);
        await _repo.UpdateAsync(college, dto.AddonIds);
        return ApiResponse<string>.SuccessResponse("College updated successfully");
    }


    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var college = await _repo.GetByIdAsync(id);
        if (college == null)
            return ApiResponse<string>.FailResponse("College not found");

        await _repo.DeleteAsync(college);
        return ApiResponse<string>.SuccessResponse("College deleted successfully");
    }

    private bool IsValidLocation(double? lat, double? lng)
    {
        if (lat == null && lng == null)
            return true; // allowed

        if (lat == null || lng == null)
            return false;

        if (lat < -90 || lat > 90)
            return false;

        if (lng < -180 || lng > 180)
            return false;

        return true;
    }

}
