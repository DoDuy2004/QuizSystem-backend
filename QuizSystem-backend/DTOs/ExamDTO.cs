using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class ExamDto
    {
        public Guid Id { get; set; }
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMinutes { get; set; }
        public int NoOfQuestions { get; set; }
        public Status Status { get; set; }
        public Guid? RoomExamId { get; set; }

        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }

        public virtual RoomExamDto? RoomExam { get; set; }
        public virtual List<ExamQuestionDto>? ExamQuestions { get; set; } = null!;
        //public virtual TeacherDto? Teacher { get; set; } = null!;
        public virtual Subject? Subject { get; set; } = null!;
    }
}
