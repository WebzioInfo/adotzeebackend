using Adotzee_Backend.Data;
using Adotzee_Backend.DTOs.RecommendationDTOs;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Adotzee_Backend.Helpers;
using System.Linq;

namespace Adotzee_Backend.Repository.RecommendationRepos
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly AppDbContext _context;

        public RecommendationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Course> Courses, IEnumerable<College> Colleges, IEnumerable<AddonCourse> Addons)> GetRecommendationsAsync(RecommendationRequestDTO request)
        {
            var keywords = RecommendationScoringHelper.ExpandKeywords(request.Interests);
            var location = request.Location?.ToLower().Trim() ?? string.Empty;

            // 1. Single-pass Popularity Aggregation (Optimization for Scale)
            var leadData = await _context.Leads
                .AsNoTracking()
                .Where(l => !string.IsNullOrEmpty(l.CollegeInterested) || !string.IsNullOrEmpty(l.CourseInterested))
                .Select(l => new { l.CollegeInterested, l.CourseInterested })
                .ToListAsync();

            var collegePopularity = leadData
                .Where(l => !string.IsNullOrEmpty(l.CollegeInterested))
                .GroupBy(l => l.CollegeInterested!.ToLower())
                .ToDictionary(g => g.Key, g => g.Count());

            var coursePopularity = leadData
                .Where(l => !string.IsNullOrEmpty(l.CourseInterested))
                .GroupBy(l => l.CourseInterested!.ToLower())
                .ToDictionary(g => g.Key, g => g.Count());

            // 2. Filter Candidates (Optimized for SQL translation)
            // Note: We use a simplified SQL filter first, then refine in memory for complex hybrid scoring.
            var streamMatch = Enum.TryParse<StreamType>(request.PreferredStream, true, out var sEnum);
            var preferredTypeMatch = Enum.TryParse<CourseType>(request.PreferredCourseType, true, out var tEnum);

            var courseCandidates = await _context.Courses
                .AsNoTracking()
                .Where(c => c.Name.Contains(request.Interests) || 
                            (streamMatch && c.Stream == sEnum))
                .ToListAsync();

            var collegeCandidates = await _context.Colleges
                .AsNoTracking()
                .Where(c => (c.Address != null && c.Address.Contains(location)) ||
                            c.IsRecommended == true)
                .ToListAsync();

            // 3. Multi-Factor Scoring
            var scoredCourses = courseCandidates.Select(c => new
            {
                Course = c,
                Score = RecommendationScoringHelper.CalculateCourseScore(
                    c, keywords, request.PreferredStream ?? "", request.PreferredCourseType ?? "", request.PreferredDuration ?? "", coursePopularity)
            })
            .OrderByDescending(x => x.Score)
            .Take(8)
            .Select(x => x.Course)
            .ToList();

            var scoredColleges = collegeCandidates.Select(c => new
            {
                College = c,
                Score = RecommendationScoringHelper.CalculateCollegeScore(c, keywords, location, collegePopularity)
            })
            .OrderByDescending(x => x.Score)
            .Take(8)
            .Select(x => x.College)
            .ToList();

            // Addons logic (Filter by relevance to recommended courses)
            var recommendedCourseIds = scoredCourses.Select(c => c.Id).ToList();
            var addonCandidates = await _context.AddonCourses
                .AsNoTracking()
                .Include(a => a.AddonColleges)
                .Where(a => keywords.Any(k => a.Name.Contains(k)) || recommendedCourseIds.Contains(a.CourseId))
                .ToListAsync();

            var recommendedCollegeIds = scoredColleges.Select(c => c.Id).ToList();
            var scoredAddons = addonCandidates.Select(a => new
            {
                Addon = a,
                Score = RecommendationScoringHelper.CalculateAddonScore(a, keywords, recommendedCollegeIds, coursePopularity)
            })
            .OrderByDescending(x => x.Score)
            .Take(8)
            .Select(x => x.Addon)
            .ToList();

            return (scoredCourses, scoredColleges, scoredAddons);
        }
    }
}
