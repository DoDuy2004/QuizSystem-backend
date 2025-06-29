using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class ExamOptionsDto
    {
        public string chapterId { get; set; } = string.Empty;
        public string difficulty { get; set; } = string.Empty;
        public string numberOfQuestions { get; set; } = string.Empty;
    }
}
