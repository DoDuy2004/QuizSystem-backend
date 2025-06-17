namespace QuizSystem_backend.Models
{
    public class StudentExamDetail
    {
        public Guid AnswerId { get; set; }

        public Guid QuestionId { get; set; }
        public Guid StudentExamId {get; set; }

        public virtual Answer Answer { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
        public virtual StudentExam StudentExam { get; set; } = null!;
    }
}
