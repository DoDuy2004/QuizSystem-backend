namespace QuizSystem_backend.Models
{
    public class StudentExam // bài thi
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }

        public Student Student { get; set; } = null!;
        public Exam Exam { get; set; } = null!;
    }
}
