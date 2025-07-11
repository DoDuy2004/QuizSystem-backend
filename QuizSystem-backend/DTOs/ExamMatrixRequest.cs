using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs
{
    public class ExamMatrixRequest
    {
        public Guid ExamId { get; set; } = new();
        public List<MatrixRow> Matrix { get; set; } = new();
    }

    public class MatrixRow
    {
        public Guid ChapterId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public Dictionary<Difficulty, int> DifficultyMap { get; set; } = new();
    }

}
