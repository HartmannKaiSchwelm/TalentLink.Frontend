using System.Numerics;

namespace TalentLink.Frontend.Models
{
    public class AdminDashboardDto
    {
        public List<UserStatDto> Users { get; set; }
        public int? TotalJobs { get; set; }
        public int? TotalApplications { get; set; }
        public int? TotalComments { get; set; }
        public int? TotalRatings { get; set; }
        public int? TotalTips { get; set; }
    }
}
