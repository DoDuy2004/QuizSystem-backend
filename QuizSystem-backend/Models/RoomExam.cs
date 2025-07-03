using QuizSystem_backend.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Models
{
    public class RoomExam
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Status Status { get; set; }
        public Guid CourseClassId { get; set; }

        // Navigation   
        public virtual CourseClass Course { get; set; } = null!;
        public virtual ICollection<Exam> Exams { get; set; } = null!;
    }
}
