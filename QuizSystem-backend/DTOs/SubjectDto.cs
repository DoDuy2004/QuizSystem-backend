using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class SubjectDto
    {
        public SubjectDto() { }
        public SubjectDto(Subject subject) 
        {
            Id = subject.Id;
            SubjectCode = subject.SubjectCode;
            Name = subject.Name;
            Major = subject.Major;
            Description = subject.Description;
        }
        public Guid Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
