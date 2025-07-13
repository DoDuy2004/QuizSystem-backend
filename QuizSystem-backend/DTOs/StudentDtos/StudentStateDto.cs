using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.StudentDtos
{
    public class StudentStateDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
    }
}
