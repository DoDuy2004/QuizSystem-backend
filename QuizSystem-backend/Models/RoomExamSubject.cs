namespace QuizSystem_backend.Models
{
    public class RoomExamSubject
    {
        public int RoomExamId { get; set; }
        public virtual RoomExam RoomExam { get; set; } = null!;

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; } = null!;

        public DateTime ExamDate { get; set; } // Ngày tháng thi của môn học


    }
}
