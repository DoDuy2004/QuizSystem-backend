using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Question
    {
        public Question() { }

        public Question(QuestionDto dto) 
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
            Content = dto.Content;
            Image = dto.Image!;
            Difficulty = dto.Difficulty;
            CreatedBy = dto.Teacher!.Id;
            Type = dto.Type;
            Topic = dto.Topic;
            ChapterId = dto.Chapter!.Id;
            QuestionBankId = dto.QuestionBank!.Id;
            Answers = dto.Answers!.Select(a => new Answer
            {
                Id = Guid.NewGuid(),
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                AnswerOrder = a.AnswerOrder,
                QuestionId = this.Id,
            }).ToList();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public string Type { get; set; } = null!;

        public string Difficulty { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string Topic { get; set; } = null!;
        public Guid? QuestionBankId { get; set; }
        public Guid? ChapterId { get; set; }

        // Navigation
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual ICollection<StudentExamDetail> StudentExamDetails { get; set; } = null!;
        public virtual QuestionBank QuestionBank { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = null!;
        public virtual Chapter Chapter { get; set; } = null!;
    }
}
