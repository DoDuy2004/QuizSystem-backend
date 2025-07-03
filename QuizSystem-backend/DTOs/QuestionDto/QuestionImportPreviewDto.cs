using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs
{
    public class QuestionImportPreviewDto
    {
        public int RowIndex { get; set; }
        public string? Chapter { get; set; } = "";
        public string Content { get; set; } = "";
        public string Subject { get; set; } = "";
        public TypeOfQuestion? Type { get; set; } 
        public bool IsValid { get; set; } = false;
        public string[] CorrectAnswer { get; set; } = [];
        public Enums.Difficulty? Difficulty { get; set; } = Enums.Difficulty.EASY;

        public int? CorrectSelectionCount = 0;

        public int? SelectionCount = 0;
        public List<AnswerImportPreview> Answer { get; set; } = new List<AnswerImportPreview>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
    public class AnswerImportPreview
    {
        public string Code { get; set; } = "";
        public string Content { get; set; } = "";
        public bool IsCorrect { get; set; } = false;
    }
}
