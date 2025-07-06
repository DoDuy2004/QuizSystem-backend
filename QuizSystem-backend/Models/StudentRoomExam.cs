using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentRoomExam
    {
        public Guid StudentId { get; set; }
        public Guid? RoomExamId { get; set; }

        public SubmitStatus SubmitStatus { get; set; } = SubmitStatus.NotSubmitted;
        public DateTime? SubmittedAt { get; set; }

        // Navigation
        public virtual Student Student { get; set; } = null!;
        public virtual RoomExam RoomExam { get; set; } = null!;
    }
}
