namespace TalentLink.Application.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!; // <-- string, nicht UserRole!
        public Guid? VerifiedByParentId { get; set; } = null;
    }
}