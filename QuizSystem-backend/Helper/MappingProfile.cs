using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using AutoMapper;
namespace QuizSystem_backend.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //CreateMap<QuestionBankDto, QuestionBank>()
            //    .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions != null ? src.Questions.Select(q => new Question(q)).ToList() : null))
            //    .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject!));

            //CreateMap<QuestionDto, Question>()
            //    .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers!.Select(a => new Answer
            //    {
            //        Id = a.Id == Guid.Empty ? Guid.NewGuid() : a.Id,
            //        Content = a.Content,
            //        IsCorrect = a.IsCorrect,
            //        AnswerOrder = a.AnswerOrder,
            //        QuestionId = dest.Id,
            //    }).ToList()));
            // Exam mapping
            //CreateMap<ExamDto, Exam>()
            //    .ForMember(dest => dest.RoomExamId, opt =>
            //                                          opt.Condition(src => src.RoomExamId != null));
            //CreateMap<Exam, ExamDto>()
            //    .ForMember(dest => dest.RoomExamId, opt =>
            //                                          opt.Condition(src => src.RoomExamId != null));
            CreateMap<Exam, ExamDto>().ReverseMap();
            CreateMap<RoomExam, RoomExamDto>().ReverseMap();
            CreateMap<ExamQuestion, ExamQuestionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Answer, AnswerDto>().ReverseMap();
            CreateMap<QuestionBank, QuestionBankDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();



        }

    }
}
