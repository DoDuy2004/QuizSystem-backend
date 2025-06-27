using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class QuestionBank
    {
        public QuestionBank() { }
        public QuestionBank(QuestionBankDto dto)
        {
            //var questions = dto.Questions != null ? dto.Questions.Select(q => new Question(q)).ToList() : null;

            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description!;
            Status = dto.Status;
            Questions = this.Questions;
            //Course = this.Course;
            Subject = dto.Subject!;
            //CourseClassId = dto.CourseClassId;
            TeacherId = dto.TeacherId;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public Status Status { get; set; }
        public Guid TeacherId { get; set; }
        public string Subject { get; set; } = null!;
        public virtual ICollection<Question>? Questions { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        //public virtual Subject Subject { get; set; } = null!;
        //public virtual CourseClass Course { get; set; } = null!;
    }
}
