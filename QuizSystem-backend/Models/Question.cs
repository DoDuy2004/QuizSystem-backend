using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public string Type { get; set; } = null!;

        public string Difficulty { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string Topic { get; set; } = null!;
        public Guid QuestionBankId { get; set; }
        public Guid ChapterId { get; set; }

        // Navigation
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual ICollection<StudentExamDetail> StudentExamDetails { get; set; } = null!;
        public virtual QuestionBank QuestionBank { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual Chapter Chapter { get; set; } = null!;
    }
}
