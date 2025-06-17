using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class CourseClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int Credit { get; set; } // số tín chỉ
        public Status Status { get; set; }
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }

        // Navigation
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<RoomExam> RoomExams { get; set;} = null!;

    }
}
