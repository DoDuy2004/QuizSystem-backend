using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class ChapterDto
    {
        public ChapterDto() { }
        public ChapterDto(Chapter chapter) 
        { 
            Id = chapter.Id;
            Name = chapter.Name;
            Subject = chapter.Course.Subject;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid CourseClassId { get; set; } 

        public string Subject { get; set; } = null!;    
    }
}
