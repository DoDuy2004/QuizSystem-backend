using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class QuestionBankDto
    {
        public QuestionBankDto () { }
        public QuestionBankDto(QuestionBank qb) 
        {
            Id = qb.Id;
            Name = qb.Name;
            Description = qb.Description;
            Status = qb.Status;
            Questions = qb.Questions.Select(q => new QuestionDto(q)).ToList();
            Subject = qb.Course.Subject;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string Subject { get; set; } = string.Empty;
        public List<QuestionDto> Questions { get; set; } = null!;
    }
}
