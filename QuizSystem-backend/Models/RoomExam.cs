using QuizSystem_backend.Enums;

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
        public Guid SubjectId { get; set; } // Assuming SubjectId is a Guid, adjust as necessary

        // Navigation   
        public virtual CourseClass Course { get; set; } = null!;
        public virtual ICollection<Exam> Exams { get; set; } = null!;


        public virtual ICollection<StudentExam> StudentExams { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
