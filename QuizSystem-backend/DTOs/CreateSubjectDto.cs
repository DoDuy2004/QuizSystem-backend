using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class CreateSubjectDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        //public Guid FacutlyId { get; set; }
        public Status Status { get; set; }

        // Navigation
        //public virtual Facutly Facutly { get; set; } = null!;
        //public virtual ICollection<CourseClass> Courses { get; set; } = null!;
        //public virtual ICollection<RoomExam> RoomExams { get; set; } = null!;
        //public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = null!;
        public virtual ICollection<ChapterDto> Chapters { get; set; } = null!;
        //public virtual ICollection<Exam> Exams { get; set; } = null!;
    }
}
