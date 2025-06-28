using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class ExamQuestionDto
    {
        public Guid ExamId { get; set; }

        public Guid QuestionId { get; set; }
        public int Order { get; set; } // thứ tự trong Exam
        public float Score { get; set; }

        // Navigation
        public virtual ExamDto Exam { get; set; } = null!;

        public virtual QuestionDto Question { get; set; } = null!;
    }

}
