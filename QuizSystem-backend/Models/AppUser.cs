using Microsoft.AspNetCore.Identity;
using QuizSystem_backend.Enums;
using System.Data;

namespace QuizSystem_backend.Models
{
    public class AppUser:IdentityUser<Guid>
    {
        
        public string FullName { get; set; } = string.Empty;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
