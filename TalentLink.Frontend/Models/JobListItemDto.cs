namespace TalentLink.Frontend.Models
{
    public class JobListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal PricePerHour { get; set; }
        public bool IsBoosted { get; set; }

        public string ZipCode { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Category { get; set; }
        public string CategoryImage { get; set; }
        public string CreatedBy { get; set; }
    }
}
