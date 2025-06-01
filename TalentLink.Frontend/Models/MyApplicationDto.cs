namespace TalentLink.Frontend.Models
{
    public class MyApplicationDto
    {
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime AppliedAt { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
