namespace TalentLink.Frontend.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal PricePerHour { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsBoosted { get; set; }
        public JobCategory Category { get; set; }
        public Guid CreatedById { get; set; }
    }
}
