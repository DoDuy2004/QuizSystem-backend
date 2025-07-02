using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.StudentDtos
{
    public class StudentImportDto
    {
        public int IndexCode { get; set; }
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? StudentCode { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public Status? status { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();

    }

}
