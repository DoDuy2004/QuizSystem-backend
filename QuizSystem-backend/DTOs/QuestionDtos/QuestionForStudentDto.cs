using QuizSystem_backend.DTOs.AnswerDtos;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.QuestionDtos
{
    public class QuestionForStudentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public TypeOfQuestion? Type { get; set; } = null!;
        public List<AnswerForStudentDto> Answers { get; set; } = new();

    }
}
