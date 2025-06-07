namespace QuizSystem_backend.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int Status { get; set; }

        // Navigation
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = new List<TeacherSubjectClass>();

    }
}
