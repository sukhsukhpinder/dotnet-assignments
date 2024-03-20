namespace EntityFrameworkAPI.Services.DTOs
{
    [System.Serializable]
    public class XmlStudent
    {
        public Guid StudentId { get; set; }

        public string StudentName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public string ImagePath { get; set; }
        public string Role { get; set; } = "Student";
    }
}
