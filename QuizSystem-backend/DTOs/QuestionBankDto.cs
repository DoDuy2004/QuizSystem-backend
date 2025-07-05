using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class QuestionBankDto
    {
        public QuestionBankDto () { }
        public QuestionBankDto(QuestionBank qb) 
        {
            var questions = qb.Questions != null ? qb.Questions.Select(q => new QuestionDto(q)).ToList() : null;
            //var course = qb.Course != null ? new CourseClassDto(qb.Course) : null;
            //var teacher = qb.Teacher != null ? new TeacherDto(qb.Teacher) : null;

            Id = qb.Id;
            Name = qb.Name;
            Description = qb.Description;
            Status = qb.Status;
            //TeacherId = qb.TeacherId;
            //Questions = questions!;
            //Course = course;
            //CourseClassId = qb.CourseClassId;
            NoOfQuestions = questions!.Count();
            //Teacher = teacher;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Status Status { get; set; }
        public int NoOfQuestions { get; set; }
        
        public Guid TeacherId { get; set; }

        //public List<QuestionDto>? Questions { get; set; } = null!;
        public TeacherDto? Teacher { get; set; } = null!;
    }
}
