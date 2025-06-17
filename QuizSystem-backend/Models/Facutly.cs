using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Facutly
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FacutlyCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Status Status { get; set; }

        // Navigation
        public virtual ICollection<Subject> Subjects { get; set; } = null!;
        public virtual ICollection<Teacher> Teachers { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = null!;
    }
}
