namespace QuizSystem_backend.Models
{
    public class Teacher : User
    {
        public string TeacherCode { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; }
        public string Facutly { get; set; } = string.Empty;

        // Navigation
        //public virtual Facutly Facutly { get; set; } = null!;

        public virtual ICollection<NotificationForCourseClass> NotificationForCourseClasses { get; set; } = new List<NotificationForCourseClass>();
    }
}
