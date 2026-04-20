namespace Adotzee_Backend.DTOs.LeadDTOs
{
    public class LeadDashboardStatsDTO
    {
        public int TotalLeads { get; set; }
        public int TotalNew { get; set; }
        public int TotalContacted { get; set; }
        public int TotalConverted { get; set; }
        public int TotalRejected { get; set; }
        public double ConversionRate { get; set; }
        public int LeadsToday { get; set; }
        public int LeadsThisMonth { get; set; }

        public Dictionary<string, int> LeadsBySource { get; set; } = new();
        public Dictionary<string, int> LeadsByStatus { get; set; } = new();
        public Dictionary<string, int> LeadsByPriority { get; set; } = new();
        public List<MonthWiseDTO> MonthlyLeads { get; set; } = new();
    }
}
