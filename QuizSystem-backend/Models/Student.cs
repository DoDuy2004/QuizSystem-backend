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
        public string Facutly { get; set; } = null!;

        // Navigation
        //public virtual Facutly Facutly { get; set; } = null!;
    }
}
