using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Question
    {
        public Question() { }

        public Question(QuestionDto dto) 
        {
            var chapterId = dto.Chapter != null ? dto.Chapter.Id : dto.ChapterId;
            var createdBy = dto.Teacher != null ? dto.Teacher.Id : dto.CreatedBy;
            var questionBankId = dto.QuestionBank != null ? dto.QuestionBank.Id : dto.QuestionBankId;

            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
            Content = dto.Content;
            Image = dto.Image!;
            Difficulty = dto.Difficulty;
            CreatedBy = createdBy;
            Type = dto.Type;
            Topic = dto.Topic;
            //ChapterId = dto.Chapter!.Id;
            ChapterId = chapterId;
            Status = dto.Status;
            //QuestionBankId = dto.QuestionBank!.Id;
            QuestionBankId = questionBankId;
            Answers = dto.Answers!.Select(a => new Answer
            {
                Id = a.Id == Guid.Empty ? Guid.NewGuid() : a.Id,
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
