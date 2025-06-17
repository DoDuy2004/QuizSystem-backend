using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Answer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int AnswerOrder { get; set; }
        public Status Status { get; set; }

        //public bool isChoose { get; set; }

        public Guid QuestionId { get; set; }

        // Navigation
        public virtual Question Question { get; set; } = null!;
    }
}
