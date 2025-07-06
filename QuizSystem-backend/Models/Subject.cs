using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Subject
    {
        public Subject() { }
        public Subject(SubjectDto dto)
        {
            Id = dto.Id;
            SubjectCode = dto.SubjectCode;
            Name = dto.Name;
            Description = dto.Description;
            Major = dto.Major;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        //public Guid FacutlyId { get; set; }
        public string Major { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }

        // Navigation
        //public virtual Facutly Facutly { get; set; } = null!;
        public virtual ICollection<CourseClass> Courses { get; set; } = null!;
        public virtual ICollection<RoomExam> RoomExams { get; set; } = null!;
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = null!;
        public virtual ICollection<Exam> Exams { get; set; } = null!;
    }
}
