namespace QuizSystem_backend.Models
{
    public class GeneratedExam
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ExamCode { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = null!;

        public Exam Exam { get; set; } = null!;
    }
}
