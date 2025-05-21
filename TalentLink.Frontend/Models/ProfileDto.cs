namespace TalentLink.Frontend.Models
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; }

        public List<Job>? CreatedJobs { get; set; } // nur für Seniors
        public List<Job>? AppliedJobs { get; set; } // nur für Students (wenn Bewerbungslogik da ist)
        public List<VerifiedStudent>? VerifiedStudents { get; set; } // nur für Parents
    }
}
