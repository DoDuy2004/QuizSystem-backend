namespace QuizSystem_backend.DTOs.StudentExamDto
{
    public class StudentExamResultDto
    {
        public float Grade { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectCount { get; set; }
        public List<QuestionResultDto> QuestionResults { get; set; } = new();
    }
}
