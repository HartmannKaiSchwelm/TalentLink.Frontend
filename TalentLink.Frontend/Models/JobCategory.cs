namespace TalentLink.Frontend.Models
{
    public class JobCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
