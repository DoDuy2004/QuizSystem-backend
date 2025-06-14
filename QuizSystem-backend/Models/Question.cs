namespace QuizSystem_backend.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public string Type { get; set; } = null!;

        public string Difficulty { get; set; } = string.Empty;
        public int Status { get; set; }
        public int QuestionBankId { get; set; }

        public string Topic { get; set; } = null!;
        // Navigation
        public virtual Teacher Teacher { get; set; } = null!;
        //public QuestionType QuestionType { get; set; } = null!;
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
        public virtual QuestionBank QuestionBank { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
