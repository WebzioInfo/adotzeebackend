using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs.LeadDTOs;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.LeadRepos
{
    public class LeadRepository : ILeadRepository
    {
        private readonly AppDbContext _context;

        public LeadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Lead> Leads, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? search = null, string? source = null, string? status = null)
        {
            var query = _context.Leads.Where(l => !l.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(l => l.FullName.ToLower().Contains(lowerSearch) || l.PhoneNumber.Contains(lowerSearch));
            }

            if (!string.IsNullOrWhiteSpace(source) && Enum.TryParse<LeadSource>(source, true, out var parsedSource))
            {
                query = query.Where(l => l.Source == parsedSource);
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LeadStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(l => l.Status == parsedStatus);
            }

            int totalCount = await query.CountAsync();

            var leads = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (leads, totalCount);
        }

        public async Task<(List<Lead> Leads, bool HasMore, int? NextCursor)> GetAllPagedAsync(int? cursor = null, int pageSize = 10, string? search = null, string? source = null, string? status = null)
        {
            var query = _context.Leads.Where(l => !l.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(l => l.FullName.ToLower().Contains(lowerSearch) || l.PhoneNumber.Contains(lowerSearch));
            }

            if (!string.IsNullOrWhiteSpace(source) && Enum.TryParse<LeadSource>(source, true, out var parsedSource))
            {
                query = query.Where(l => l.Source == parsedSource);
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LeadStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(l => l.Status == parsedStatus);
            }

            // Cursor-based filter (assume newest first, so Id < cursor)
            if (cursor.HasValue)
            {
                query = query.Where(l => l.Id < cursor.Value);
            }

            var leads = await query
                .OrderByDescending(l => l.Id)
                .Take(pageSize + 1) // Fetch one extra to see if there's more
                .ToListAsync();

            bool hasMore = leads.Count > pageSize;
            var resultLeads = hasMore ? leads.Take(pageSize).ToList() : leads;
            int? nextCursor = hasMore ? resultLeads.Last().Id : null;

            return (resultLeads, hasMore, nextCursor);
        }

        public async Task<Lead?> GetByIdAsync(int id)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
        }

        public async Task<Lead> AddAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<Lead> UpdateAsync(Lead lead)
        {
            lead.UpdatedAt = DateTime.UtcNow;
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<Lead> DeleteAsync(Lead lead)
        {
            lead.IsDeleted = true;
            lead.UpdatedAt = DateTime.UtcNow;
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        // --- OPTIMIZED DASHBOARD STATS ---

        public async Task<LeadDashboardStatsDTO> GetDashboardStatsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            
            // Basic counts in parallel tasks to avoid sequential blocking
            var leadsQuery = _context.Leads.Where(l => !l.IsDeleted);
            
            var counts = await leadsQuery.GroupBy(l => l.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status.ToString(), v => v.Count);
                
            var sources = await leadsQuery.GroupBy(l => l.Source)
                .Select(g => new { Source = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Source, v => v.Count);
                
            var priorities = await leadsQuery.GroupBy(l => l.Priority)
                .Select(g => new { Priority = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Priority, v => v.Count);
                
            var todayCount = await leadsQuery.CountAsync(l => l.CreatedAt.Date == today);
            var monthCount = await leadsQuery.CountAsync(l => l.CreatedAt.Month == currentMonth && l.CreatedAt.Year == currentYear);
            var trend = await GetMonthlyTrendAsync();

            int totalLeads = counts.Values.Sum();
            int convertedCount = counts.GetValueOrDefault(LeadStatus.Converted.ToString(), 0);

            return new LeadDashboardStatsDTO
            {
                TotalLeads = totalLeads,
                TotalNew = counts.GetValueOrDefault(LeadStatus.New.ToString(), 0),
                TotalContacted = counts.GetValueOrDefault(LeadStatus.Contacted.ToString(), 0),
                TotalConverted = convertedCount,
                TotalRejected = counts.GetValueOrDefault(LeadStatus.Rejected.ToString(), 0),
                ConversionRate = totalLeads == 0 ? 0 : Math.Round((double)convertedCount / totalLeads * 100, 2),
                LeadsToday = todayCount,
                LeadsThisMonth = monthCount,
                LeadsBySource = sources,
                LeadsByStatus = counts,
                LeadsByPriority = priorities,
                MonthlyLeads = trend
            };
        }

        // --- LEGACY METHODS (to be removed after verify) ---

        public async Task<int> GetTotalLeadsAsync()
        {
            return await _context.Leads.CountAsync(l => !l.IsDeleted);
        }

        public async Task<int> GetTotalNewLeadsAsync()
        {
            return await _context.Leads.CountAsync(l => !l.IsDeleted && l.Status == LeadStatus.New);
        }

        public async Task<int> GetTotalContactedLeadsAsync()
        {
            return await _context.Leads.CountAsync(l => !l.IsDeleted && l.Status == LeadStatus.Contacted);
        }

        public async Task<int> GetTotalConvertedLeadsAsync()
        {
            return await _context.Leads.CountAsync(l => !l.IsDeleted && l.Status == LeadStatus.Converted);
        }

        public async Task<int> GetTotalRejectedLeadsAsync()
        {
            return await _context.Leads.CountAsync(l => !l.IsDeleted && l.Status == LeadStatus.Rejected);
        }

        public async Task<double> GetConversionRateAsync()
        {
            int total = await GetTotalLeadsAsync();
            if (total == 0) return 0;
            int converted = await GetTotalConvertedLeadsAsync();
            return Math.Round((double)converted / total * 100, 2);
        }

        public async Task<int> GetLeadsTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Leads
                .CountAsync(l => !l.IsDeleted && l.CreatedAt.Date == today);
        }

        public async Task<int> GetLeadsThisMonthAsync()
        {
            var today = DateTime.UtcNow;
            return await _context.Leads
                .CountAsync(l => !l.IsDeleted && l.CreatedAt.Month == today.Month && l.CreatedAt.Year == today.Year);
        }

        public async Task<Dictionary<string, int>> GetLeadsBySourceAsync()
        {
            return await _context.Leads
                .Where(l => !l.IsDeleted)
                .GroupBy(l => l.Source)
                .Select(g => new { Source = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Source, v => v.Count);
        }

        public async Task<Dictionary<string, int>> GetLeadsByStatusAsync()
        {
            return await _context.Leads
                .Where(l => !l.IsDeleted)
                .GroupBy(l => l.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count);
        }

        public async Task<Dictionary<string, int>> GetLeadsByPriorityAsync()
        {
            return await _context.Leads
                .Where(l => !l.IsDeleted)
                .GroupBy(l => l.Priority)
                .Select(g => new { Priority = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(k => k.Priority, v => v.Count);
        }

        public async Task<List<MonthWiseDTO>> GetMonthlyTrendAsync()
        {
            var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-11); // Last 12 months including current
            var startDate = new DateTime(twelveMonthsAgo.Year, twelveMonthsAgo.Month, 1);

            var rawData = await _context.Leads
                .Where(l => !l.IsDeleted && l.CreatedAt >= startDate)
                .GroupBy(l => new { l.CreatedAt.Year, l.CreatedAt.Month })
                .Select(g => new 
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            // Fill in missing months with 0
            var result = new List<MonthWiseDTO>();
            for (int i = 0; i < 12; i++)
            {
                var currentDate = startDate.AddMonths(i);
                var monthData = rawData.FirstOrDefault(d => d.Year == currentDate.Year && d.Month == currentDate.Month);
                
                result.Add(new MonthWiseDTO
                {
                    Month = currentDate.ToString("MMM yyyy"), // e.g., "Jan 2024"
                    Count = monthData?.Count ?? 0
                });
            }

            return result;
        }
    }
}
