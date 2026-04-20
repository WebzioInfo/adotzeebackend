using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;
using Adotzee_Backend.Repository.LeadRepos;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace Adotzee_Backend.Services.LeadServices
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _repo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string DashboardStatsCacheKey = "LeadDashboardStats";

        public LeadService(ILeadRepository repo, IMapper mapper, IMemoryCache cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse<object>> GetAllAsync(int? cursor = null, int pageSize = 10, string? search = null, string? source = null, string? status = null)
        {
            var result = await _repo.GetAllPagedAsync(cursor, pageSize, search, source, status);
            var dto = _mapper.Map<List<LeadResponseDTO>>(result.Leads);

            return ApiResponse<object>.SuccessResponse(new 
            {
                Data = dto,
                HasMore = result.HasMore,
                NextCursor = result.NextCursor,
                PageSize = pageSize
            });
        }

        public async Task<ApiResponse<LeadResponseDTO>> GetByIdAsync(int id)
        {
            var lead = await _repo.GetByIdAsync(id);
            if (lead == null)
                return ApiResponse<LeadResponseDTO>.FailResponse("Lead not found");

            var dto = _mapper.Map<LeadResponseDTO>(lead);
            return ApiResponse<LeadResponseDTO>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<string>> CreateAsync(LeadCreateDTO dto)
        {
            var lead = _mapper.Map<Lead>(dto);
            await _repo.AddAsync(lead);
            _cache.Remove(DashboardStatsCacheKey); // Invalidate cache on new lead
            return ApiResponse<string>.SuccessResponse("Lead created successfully");
        }

        public async Task<ApiResponse<string>> UpdateAsync(LeadUpdateDTO dto)
        {
            var lead = await _repo.GetByIdAsync(dto.Id);
            if (lead == null)
                return ApiResponse<string>.FailResponse("Lead not found");

            _mapper.Map(dto, lead);
            await _repo.UpdateAsync(lead);
            _cache.Remove(DashboardStatsCacheKey); // Invalidate cache on update
            return ApiResponse<string>.SuccessResponse("Lead updated successfully");
        }

        public async Task<ApiResponse<string>> UpdateStatusAsync(int id, LeadStatus status)
        {
            var lead = await _repo.GetByIdAsync(id);
            if (lead == null)
                return ApiResponse<string>.FailResponse("Lead not found");

            lead.Status = status;
            await _repo.UpdateAsync(lead);
            _cache.Remove(DashboardStatsCacheKey); // Invalidate cache on status change
            return ApiResponse<string>.SuccessResponse("Lead status updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var lead = await _repo.GetByIdAsync(id);
            if (lead == null)
                return ApiResponse<string>.FailResponse("Lead not found");

            await _repo.DeleteAsync(lead);
            _cache.Remove(DashboardStatsCacheKey); // Invalidate cache on deletion
            return ApiResponse<string>.SuccessResponse("Lead deleted successfully (Soft delete)");
        }

        public async Task<ApiResponse<LeadDashboardStatsDTO>> GetDashboardStatsAsync()
        {
            if (!_cache.TryGetValue(DashboardStatsCacheKey, out LeadDashboardStatsDTO? stats) || stats == null)
            {
                stats = await _repo.GetDashboardStatsAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                
                _cache.Set(DashboardStatsCacheKey, stats, cacheOptions);
            }

            return ApiResponse<LeadDashboardStatsDTO>.SuccessResponse(stats!);
        }
    }
}
