using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.UserDtos
{
    public class AddUserDtos
    {
        public string FullName { get; set; } = string.Empty;
        public string? Code {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; }
       

    }
}
