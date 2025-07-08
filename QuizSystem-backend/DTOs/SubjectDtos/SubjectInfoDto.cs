using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.SubjectDtos
{
    public class SubjectInfoDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
    }
}
