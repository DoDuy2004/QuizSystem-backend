using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class CourseClass
    {
        public CourseClass() {  }
        public CourseClass(CourseClassDto dto)
        {
            Id = dto.Id;
            ClassCode = dto.ClassCode;
            Name = dto.Name;
            Credit = dto.Credit;
            Status = dto.Status;
            TeacherId = dto.TeacherId;
            SubjectId = dto.SubjectId;
            Teacher = this.Teacher;
        }
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
        public virtual ICollection<Chapter> Chapters { get; set; } = null!;
        public virtual ICollection<RoomExam> RoomExams { get; set;} = null!;
        public ICollection<StudentCourseClass> Students { get; set; } = null!;


    }
}
