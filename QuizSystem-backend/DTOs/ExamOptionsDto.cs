using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class ExamOptionsDto
    {
        public Guid chapterId { get; set; }
        public Difficulty difficulty { get; set; }
        public string numberOfQuestions { get; set; } = string.Empty;
    }
}
