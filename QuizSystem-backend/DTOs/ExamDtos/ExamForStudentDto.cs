using QuizSystem_backend.DTOs.QuestionDtos;

namespace QuizSystem_backend.DTOs.ExamDtos
{
    public class ExamForStudentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int NoOfQuestions { get; set; }
        public List<QuestionForStudentDto> Questions { get; set; } = new List<QuestionForStudentDto>();
    }
}
