using NuGet.DependencyResolver;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Exam // đề thi
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMinutes { get; set; }
        public int NoOfQuestions { get; set; }
        //public float TotalScore { get; set; }
        public Guid? RoomExamId { get; set; }
        public Status Status { get; set; }

        //public Guid SubjectId { get; set; }
        //public Guid TeacherId { get; set; }

        // Navigation
        public virtual RoomExam RoomExam { get; set; } = null!;    
        //public virtual Teacher? Teacher { get; set; } = null!;
        //public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = null!;
       
    }
}
