namespace QuizSystem_backend.DTOs.StudentExamDto
{
    public class SubmitAnswerDto
    {
        public Guid QuestionId { get; set; }
        public List<Guid> AnswerIds { get; set; } = new();
    }
}
