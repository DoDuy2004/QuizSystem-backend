namespace QuizSystem_backend.DTOs.ExamDtos
{
    public class SearchExam
    {
        public Guid Id { get; set; }
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
