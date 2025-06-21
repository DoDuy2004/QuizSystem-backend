using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Role Role { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string Facutly { get; set; } = string.Empty;
    }
}
