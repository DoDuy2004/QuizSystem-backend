using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.ChapterDtos
{
    public class ChapterInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }

    }
}
