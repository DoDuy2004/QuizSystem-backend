namespace QuizSystem_backend.DTOs
{
    public class ChapterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string Subject { get; set; } = null!;
    }
}
