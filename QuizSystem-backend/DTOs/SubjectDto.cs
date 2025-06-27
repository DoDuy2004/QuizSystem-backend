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
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
