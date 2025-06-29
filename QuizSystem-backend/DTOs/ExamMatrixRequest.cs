using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs
{
    public class ExamMatrixRequest
    {
        public ExamDto Exam { get; set; } = new();
        public List<MatrixRow> Matrix { get; set; } = new();
    }

    public class MatrixRow
    {
        public Guid ChapterId { get; set; }

        // Key: Difficulty ("Easy", "Medium", "Hard")
        // Value: Số câu hỏi
        public Dictionary<Difficulty, int> DifficultyMap { get; set; } = new();
    }

}
