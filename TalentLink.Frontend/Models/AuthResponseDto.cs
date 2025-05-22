namespace TalentLink.Frontend.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();

    }
}
