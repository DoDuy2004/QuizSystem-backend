namespace QuizSystem_backend.DTOs
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public int AnswerOrder { get; set; }

        public Guid QuestionId { get; set; }
    }
}
