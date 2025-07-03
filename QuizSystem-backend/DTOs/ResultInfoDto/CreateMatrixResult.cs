namespace QuizSystem_backend.DTOs.ResultInfoDto
{
    public class CreateMatrixResult
    {
        public bool Success { get; set; }
        public ExamDto? Exam { get; set; }
        public string ErrorMessages { get; set; } ="";
    }
}
