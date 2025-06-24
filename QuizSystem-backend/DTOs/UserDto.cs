using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{    public class UserDto
    {
        public UserDto(User user)
        {
            string userRole = ((Role)user.Role).ToString();
            string status = ((Status)user.Status).ToString();

            Id = user.Id;
            Username = user.Username;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
            DateOfBirth = user.DateOfBirth;
            AvatarUrl = user.AvatarUrl;
            Status = status;
            CreatedAt = user.CreatedAt;
            Role = userRole;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = null!;
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }
    }

}
