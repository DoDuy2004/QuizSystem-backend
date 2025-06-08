namespace QuizSystem_backend.Models
{
    public class Teacher : User
    {
        public string TeacherCode { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; }
        public int DepartmentId { get; set; }

        // Navigation
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = new List<TeacherSubjectClass>();
    }
}
