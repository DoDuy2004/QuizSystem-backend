namespace QuizSystem_backend.Models
{
    public class Student: User
    {
        public string StudentCode { get; set; } = string.Empty;
        public bool isFirstTimeLogin { get; set; }
        public int ClassId { get; set; }

        // Navigation
        public virtual Class Class { get; set; } = null!;
    }
}
