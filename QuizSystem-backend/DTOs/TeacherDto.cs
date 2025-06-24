using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class TeacherDto : UserDto
    {
        public TeacherDto() {}
        public TeacherDto(Teacher teacher) : base(teacher)
        {
            IsFirstTimeLogin = teacher.IsFirstTimeLogin;
            Facutly = teacher.Facutly;
        }
        public bool IsFirstTimeLogin { get; set; }
        public string? Facutly { get; set; } = null!;
    }
}
