namespace QuizSystem_backend.Models
{
    public class QuestionBank
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string Description {  get; set; } = string.Empty;
        public virtual ICollection<Question> questions { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
