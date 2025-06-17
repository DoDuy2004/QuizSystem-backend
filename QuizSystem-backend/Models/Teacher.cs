namespace QuizSystem_backend.Models
{
    public class Teacher : User
    {
        public string TeacherCode { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; }
        public Guid FacutlyId { get; set; }

        // Navigation
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual Facutly Department { get; set; } = null!;
        public virtual ICollection<CourseClass> CourseClasses { get; set; } = null!;
    }
}
