using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{   public class UserDto
    {
        public UserDto() { }
        public UserDto(AppUser user)
        {
            
            string status = ((Status)user.Status).ToString();

            Id = user.Id;
            Username = user.UserName;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
            DateOfBirth = user.DateOfBirth;
            AvatarUrl = user.AvatarUrl;
            Status = status;
            CreatedAt = user.CreatedAt;
           
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; } = null!;
        public string? Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
       
    }

}
