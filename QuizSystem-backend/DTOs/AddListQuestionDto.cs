namespace QuizSystem_backend.DTOs
{
    public class AddListQuestionDto
    {
        public IEnumerable<QuestionScore> QuestionScores { get; set; } = null!;
        public Guid examId { get; set; }

    }
    public class  QuestionScore
    {
        public QuestionDto Question { get; set; } = null!;
        public float Score { get; set; }

    }
}
