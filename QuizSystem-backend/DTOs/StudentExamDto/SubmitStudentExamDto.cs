namespace QuizSystem_backend.DTOs.StudentExamDto
{
    public class SubmitStudentExamDto
    {
        public Guid ExamId { get; set; }
        public Guid StudentId { get; set; }
        public Guid? RoomId { get; set; } // Nếu có phòng thi
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }
}
