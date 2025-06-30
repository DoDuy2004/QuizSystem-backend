namespace QuizSystem_backend.DTOs
{
    public class AddListQuestionToBankDto
    {
        public Guid QuestionBankId { get; set; }
        public List<QuestionDto> Questions { get; set; } = null!;
    }
}
