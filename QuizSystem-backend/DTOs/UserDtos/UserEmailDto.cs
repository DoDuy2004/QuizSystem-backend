namespace QuizSystem_backend.DTOs.UserDtos
{
    public class UserEmailDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

    }
    
}
