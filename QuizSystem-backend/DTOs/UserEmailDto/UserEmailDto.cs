namespace QuizSystem_backend.DTOs.UserEmailDto
{
    public class UserEmailDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

    }
}
