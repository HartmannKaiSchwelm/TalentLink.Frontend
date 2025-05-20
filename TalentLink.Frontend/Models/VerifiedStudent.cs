namespace TalentLink.Frontend.Models
{
    public class VerifiedStudent
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
        public Guid StudentId { get; set; }
    }
}
