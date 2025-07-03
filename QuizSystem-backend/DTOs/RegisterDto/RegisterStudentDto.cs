using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.RegisterDto
{
    public class RegisterStudentDto
    {
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? StudentCode { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public Status? status { get; set; }
    }
}
