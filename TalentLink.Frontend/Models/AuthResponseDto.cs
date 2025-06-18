namespace TalentLink.Frontend.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public Guid Id { get; set; } // Backend liefert Id, nicht UserId
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public Guid? VerifiedByParentId { get; set; } = null; 
        // Entferne UserId, nutze Id
        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();
    }
}
