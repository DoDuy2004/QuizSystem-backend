using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentRoomExam
    {
        public Guid StudentId { get; set; }
        public Guid RoomId { get; set; }

        public float? Grade { get; set; }

        public string? Note { get; set; } = string.Empty;

        public Status Status { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual RoomExam Room { get; set; } = null!;
    }
}
