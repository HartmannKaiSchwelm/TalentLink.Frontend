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
        public string Category { get; set; }
        public string CategoryImage { get; set; }
        public Guid CreatedById { get; set; }
        public bool IsAssigned { get; set; } = false; 
        public bool IsPaid {get; set;} = false; 
        public string? ZipCode { get; set; } = null!;
    }
}
