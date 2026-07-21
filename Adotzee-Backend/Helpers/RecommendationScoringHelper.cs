using Adotzee_Backend.Models;
using System.Text.RegularExpressions;

namespace Adotzee_Backend.Helpers
{
    public static class RecommendationScoringHelper
    {
        private static readonly Dictionary<string, string[]> SemanticMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "engineering", new[] { "tech", "b.tech", "m.tech", "computer", "civil", "mechanical", "electrical", "electronics" } },
            { "medicine", new[] { "mbbs", "doctor", "health", "pharma", "nursing", "dentistry", "bds" } },
            { "management", new[] { "mba", "bba", "business", "admin", "finance", "marketing", "hr" } },
            { "science", new[] { "b.sc", "m.sc", "physics", "chemistry", "biology", "maths" } },
            { "arts", new[] { "b.a", "humanities", "social", "literature", "history" } },
            { "commerce", new[] { "b.com", "m.com", "accounting", "ca", "cs" } }
        };

        public static string[] ExpandKeywords(string interests)
        {
            if (string.IsNullOrWhiteSpace(interests)) return Array.Empty<string>();

            var keywords = interests.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim().ToLower()).ToList();
            var expanded = new HashSet<string>(keywords, StringComparer.OrdinalIgnoreCase);

            foreach (var keyword in keywords)
            {
                if (SemanticMap.TryGetValue(keyword, out var synonyms))
                {
                    foreach (var syn in synonyms) expanded.Add(syn);
                }
            }

            return expanded.ToArray();
        }

        public static double CalculateCourseScore(
            Course course, 
            string[] keywords, 
            string preferredStream, 
            string preferredType, 
            string preferredDuration,
            Dictionary<string, int> popularity)
        {
            double score = 0;
            string name = course.Name.ToLower();
            string stream = course.Stream.ToString().ToLower();
            string type = course.Type.ToString().ToLower();
            string duration = course.Duration?.ToLower() ?? "";

            // 1. Interest Similarity (35%)
            double interestScore = 0;
            foreach (var keyword in keywords)
            {
                if (name.Contains(keyword)) interestScore += 10;
            }
            score += Math.Min(interestScore, 35); // Max 35 pts

            // 2. Stream Match (20%)
            if (!string.IsNullOrEmpty(preferredStream) && stream == preferredStream.ToLower())
                score += 20;

            // 3. Course Type Match (10%)
            if (!string.IsNullOrEmpty(preferredType) && type == preferredType.ToLower())
                score += 10;

            // 4. Duration Match (10%)
            if (!string.IsNullOrEmpty(preferredDuration) && duration.Contains(preferredDuration.ToLower()))
                score += 10;

            // 5. Popularity (5%)
            if (popularity.TryGetValue(name, out int count))
                score += Math.Min(count * 1, 5); // Capped at 5 pts

            return score;
        }

        public static double CalculateCollegeScore(
            College college, 
            string[] keywords, 
            List<string> userLocations, 
            Dictionary<string, int> popularity)
        {
            double score = 0;
            string name = college.Name?.ToLower() ?? "";
            string address = college.Address?.ToLower() ?? "";

            // 1. Interest Similarity (35%)
            double interestScore = 0;
            foreach (var keyword in keywords)
            {
                if (name.Contains(keyword)) interestScore += 10;
            }
            score += Math.Min(interestScore, 35);

            // 2. Location Relevance (15%)
            if (userLocations != null && userLocations.Any())
            {
                bool exactMatch = userLocations.Any(loc => address.Contains(loc));
                bool partialMatch = userLocations.Any(loc => IsPartialRegionMatch(address, loc));

                if (exactMatch)
                {
                    // Exact keyword in address
                    score += 15; 
                }
                else if (partialMatch)
                {
                    score += 10;
                }
            }

            // 3. Popularity (5%)
            if (popularity.TryGetValue(name, out int count))
                score += Math.Min(count * 0.5, 5);

            // 4. Recommended Flag (5%)
            if (college.IsRecommended == true)
                score += 5;

            return score;
        }

        public static double CalculateAddonScore(
            AddonCourse addon, 
            string[] keywords,
            IEnumerable<int> recommendedCollegeIds,
            Dictionary<string, int> popularity)
        {
            double score = 0;
            string name = addon.Name.ToLower();

            // 1. Interest Match
            foreach (var keyword in keywords)
            {
                if (name.Contains(keyword)) score += 20;
            }

            // 2. Popularity
            if (popularity.TryGetValue(name, out int count))
                score += Math.Min(count * 2, 10);

            // 3. Presence in Recommended Colleges (Contextual boost)
            if (addon.AddonColleges != null && addon.AddonColleges.Any(ac => recommendedCollegeIds.Contains(ac.CollegeId)))
            {
                score += 30;
            }

            return score;
        }

        private static bool IsPartialRegionMatch(string address, string location)
        {
            // Simple check for regional clusters (South/North/East/West) - extensible
            var regionalMap = new Dictionary<string, string[]>
            {
                { "kerala", new[] { "kochi", "trivandrum", "calicut", "thrissur", "kannur" } },
                { "karnataka", new[] { "bangalore", "mangaluru", "mysuru" } },
                { "maharashtra", new[] { "mumbai", "pune", "nagpur" } }
            };

            if (regionalMap.TryGetValue(location, out var cities))
            {
                return cities.Any(c => address.Contains(c));
            }

            return false;
        }
    }
}
