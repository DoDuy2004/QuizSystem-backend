using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Chapter
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Guid SubjectId { get; set; }

        public virtual ICollection<Question> Question { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
