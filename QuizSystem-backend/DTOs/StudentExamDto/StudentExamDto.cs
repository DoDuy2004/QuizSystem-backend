using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.StudentExamDto
{
    public class StudentExamDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int NoOfQuestions { get; set; }
        public Guid? RoomExamId { get; set; }
        public Status Status { get; set; }
        public Guid SubjectId { get; set; }
        public Guid UserId { get; set; }
    }
}
