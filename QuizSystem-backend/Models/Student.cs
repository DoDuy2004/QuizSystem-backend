using QuizSystem_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace QuizSystem_backend.Models
{
    public class Student: User
    {
        public string StudentCode { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; }
        public Guid FacutlyId { get; set; }

        // Navigation
        public virtual Facutly Facutly { get; set; } = null!;
    }
}
