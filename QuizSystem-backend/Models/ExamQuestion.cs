using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizSystem_backend.Models
{
    public class ExamQuestion
    {
        public Guid ExamId { get; set; }

        public Guid QuestionId { get; set; }
        public int Order { get; set; } // thứ tự trong Exam
        public float Score { get; set; }

        // Navigation
        public virtual Exam Exam { get; set; } = null!;

        public virtual Question Question { get; set; } = null!;
    }
}
