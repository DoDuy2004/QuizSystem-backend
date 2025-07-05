using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs.ExamDtos
{
    public class AddRoomExamDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Status Status { get; set; }
        
        public Guid SubjectId { get; set; } // Assuming SubjectId is a Guid, adjust as necessary
        public Guid CourseClassId { get; set; } // Assuming CourseClassId is a Guid, adjust as necessary
        public Guid ExamId { get; set; } // Assuming ExamId is a Guid, adjust as necessary

        // Navigation   

    }
}
