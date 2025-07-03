using Humanizer;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class QuestionDto
    {
        public QuestionDto () { }
        public QuestionDto(Question question) {
            var answers = question.Answers != null ? question.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                AnswerOrder = a.AnswerOrder,
                QuestionId = a.QuestionId,
            }).ToList() : null;

            var teacher = question.Teacher != null ? new TeacherDto
            {
                Id = question!.Teacher.Id,
                FullName = question.Teacher.FullName,
                Username = question.Teacher.Username,
                Email = question.Teacher.Email,
                PhoneNumber = question.Teacher.PhoneNumber,
                Gender = question.Teacher.Gender,
                DateOfBirth = question.Teacher.DateOfBirth,
            } : null;

            var chapter = question.Chapter != null ? new ChapterDto(question.Chapter) : null;

            var questionBank = question?.QuestionBank != null ? new QuestionBankDto
            {
                Id = question.QuestionBank.Id,
                Name = question.QuestionBank.Name,
                //TeacherId = question.QuestionBank.TeacherId
                //CourseClassId = question.QuestionBank.CourseClassId,
            } : null ;

            Id = question.Id;
            Content = question.Content;
            Image = question.Image;
            Difficulty = question.Difficulty;
            Type = question.Type;
            Topic = question.Topic;
            Status = question.Status;
            Teacher = teacher;
            Chapter = chapter;
            QuestionBank = questionBank;
            Answers = answers;
        }
        public Guid Id { get; set; }
        public string Topic { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public Status Status { get; set; } = Status.ACTIVE;
        public Difficulty Difficulty { get; set; }
        public Guid? ChapterId { get; set; }
        public Guid? QuestionBankId { get; set; }
        public Guid? CreatedBy { get; set; }
        public TeacherDto? Teacher { get; set; } = null!;
        public ChapterDto? Chapter { get; set; } = null!;
        public QuestionBankDto? QuestionBank { get; set; } = null!;
        public List<AnswerDto>? Answers { get; set; } = null!;
    }
}
