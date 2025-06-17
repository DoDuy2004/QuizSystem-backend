using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Subject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid FacutlyId { get; set; }
        public Status Status { get; set; }

        // Navigation
        public virtual Facutly Facutly { get; set; } = null!;
        public virtual ICollection<CourseClass> Courses { get; set; } = null!;
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = null!;
    }
}
