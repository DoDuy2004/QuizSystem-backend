using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.AuthDto
{
    public class RegisterDto
    {
        public string? Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.ACTIVE;
        public DateTime CreatedAt { get; set; }
    }
}
