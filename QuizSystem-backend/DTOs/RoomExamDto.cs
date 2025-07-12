using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class RoomExamDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }

        public Guid ExamId { get; set; }
        public Subject? Subject { get; set; }         // Object Subject
        public CourseClass? Course { get; set; }       // Object CourseClass
        public List<Exam>? Exams { get; set; }

    }
}
