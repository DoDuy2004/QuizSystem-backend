namespace QuizSystem_backend.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }

        // Navigation
        public ICollection<Subject> Subjects { get; set; } = null!;
        public virtual ICollection<Teacher> Teachers { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
