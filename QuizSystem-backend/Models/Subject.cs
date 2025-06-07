namespace QuizSystem_backend.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int Status { get; set; }

        // Navigation
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual Department Department { get; set; } = null!;
        public ICollection<ExamSessionSubject> ExamSessionSubjects { get; set; } = new List<ExamSessionSubject>();
        public virtual ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = new List<TeacherSubjectClass>();
    }
}
