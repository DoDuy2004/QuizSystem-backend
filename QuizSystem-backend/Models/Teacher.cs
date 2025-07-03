namespace QuizSystem_backend.Models
{
    public class Teacher : AppUser
    {
        public string TeacherCode { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; }
        public string Facutly { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        //public virtual Facutly Facutly { get; set; } = null!;
        public virtual ICollection<CourseClass> CourseClasses { get; set; } = null!;

        public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = null!;
        public virtual ICollection<Exam> Exams { get; set; } = null!;
    }
}
