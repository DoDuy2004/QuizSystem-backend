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
        //public Guid CourseClassId { get; set; }

        // Navigation   
        //public virtual CourseClassDto Course { get; set; } = null!;
        public virtual ICollection<StudentRoomExam> StudentsRoomExams { get; set; } = null!;
        public virtual ICollection<ExamDto> Exams { get; set; } = null!;
    }
}
