using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class StudentDto : UserDto
    {
        public StudentDto(Student student) : base(student)
        {
            StudentCode = student.StudentCode;
            IsFirstTimeLogin = student.IsFirstTimeLogin;
            Facutly = student.Facutly;
        }

        public string StudentCode { get; set; } = null!;
        public bool IsFirstTimeLogin { get; set; }
        public string Facutly { get; set; } = null!;
    }
}
