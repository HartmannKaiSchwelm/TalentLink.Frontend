namespace TalentLink.Frontend.Models
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Role { get; set; } // 0 = Student, 1 = Senior, 2 = Parent

        public string? ZipCode { get; set; } = null!;
        public string? City { get; set; } = null!; 

        public DateOnly Birthday { get; set;  }
        public string? StudentEmail { get; set; }

    }
}
