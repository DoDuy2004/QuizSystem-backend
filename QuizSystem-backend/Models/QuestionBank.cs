using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class QuestionBank
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public Status Status { get; set; }
        //public string Subject { get; set; } = null!;
        public Guid CourseClassId { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = null!;
        //public virtual Subject Subject { get; set; } = null!;
        public virtual CourseClass Course { get; set; } = null!;
    }
}
