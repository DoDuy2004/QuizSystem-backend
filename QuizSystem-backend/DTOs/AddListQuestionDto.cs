using QuizSystem_backend.Enums;

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
    //public class QuestionsAddedToExamDto
    //{
    //    public Guid Id { get; set; }
    //    public string Topic { get; set; } = null!;
    //    public string Type { get; set; } = null!;
    //    public string Content { get; set; } = null!;
    //    public string? Image { get; set; } = null!;
    //    public Status Status { get; set; } = Status.ACTIVE;
    //    public Difficulty Difficulty { get; set; }
    //    public Guid? ChapterId { get; set; }
    //    public Guid? QuestionBankId { get; set; }
    //    public Guid? CreatedBy { get; set; }
        
    //    public List<AnswerDto>? Answers { get; set; } = null!;
    //}
}
