using Humanizer;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class QuestionDto
    {
        public QuestionDto() { }
        public QuestionDto(Question question) {
            Id = question.Id;
            Content = question.Content;
            Image = question.Image;
            Difficulty = question.Difficulty;
            Type = question.Type;
            Topic = question.Topic;
            Status = question.Status;
            Teacher = new TeacherDto
            {
                Id = question.Teacher.Id,
                FullName = question.Teacher.FullName,
                Username = question.Teacher.Username,
                Email = question.Teacher.Email,
                PhoneNumber = question.Teacher.PhoneNumber,
                Gender = question.Teacher.Gender,
                DateOfBirth = question.Teacher.DateOfBirth,
            };
            Chapter = new ChapterDto
            {
                Id = question.Chapter.Id,
                Name = question.Chapter.Name,
                Subject = question.Chapter.Course.Subject
            };
            QuestionBank = new QuestionBankDto
            {
                Id = question.QuestionBank.Id,
                Name = question.QuestionBank.Name,
            };
            Answers = question.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                AnswerOrder = a.AnswerOrder,
                QuestionId = a.QuestionId,
            }).ToList();
        }
        public Guid Id { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Status Status { get; set; } = Status.ACTIVE;
        public string Difficulty { get; set; } = string.Empty;
        public TeacherDto? Teacher { get; set; } = null!;
        public ChapterDto? Chapter { get; set; } = null!;
        public QuestionBankDto? QuestionBank { get; set; } = null!;
        public List<AnswerDto>? Answers { get; set; } = null!;
    }
}
