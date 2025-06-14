namespace QuizSystem_backend.Models
{
    public class RoomExam
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Status { get; set; }

        // Navigation
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<RoomExamSubject> RoomExamSubjects { get; set; } = new List<RoomExamSubject>();
    }
}
