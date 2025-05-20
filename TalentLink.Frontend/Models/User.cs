namespace TalentLink.Frontend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Student, 
        Senior, 
        Parent
    }

    public class Student : User
    {
        public Guid? VerifiedByParentId { get; set; }
    }

    public class Senior : User
    {
        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();    
    }

    public class Parent : User
    {
        public ICollection<VerifiedStudent> VerifiedStudents { get; set; } = new List<VerifiedStudent>();
    }
}
