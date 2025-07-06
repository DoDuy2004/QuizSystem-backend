namespace QuizSystem_backend.DTOs.StudentExamDto
{
    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<Guid> CorrectAnswerIds { get; set; } = new();
        public List<Guid> StudentAnswerIds { get; set; } = new();
        public bool IsCorrect { get; set; }
    }
}
