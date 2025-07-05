using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.StudentDtos
{
    public class StudentInfoDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
        public string StudentCode { get; set; } = null!;
        public bool IsFirstTimeLogin { get; set; }
    }
}
