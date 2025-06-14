namespace QuizSystem_backend.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int AnswerOrder { get; set; }
        public int Status { get; set; }

        //public bool isChoose { get; set; }

        public int QuestionId { get; set; }

        // Navigation
        public virtual Question Question { get; set; } = null!;
    }
}
