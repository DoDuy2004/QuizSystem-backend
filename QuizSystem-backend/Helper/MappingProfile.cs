﻿using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using AutoMapper;
using QuizSystem_backend.DTOs.CourseClassDtos;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;
using QuizSystem_backend.DTOs.ChapterDtos;
namespace QuizSystem_backend.Helper
{
    public class MappingProfile : Profile
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
            CreateMap<ExamDto, Exam>()
                .ForMember(dest => dest.NoOfQuestions,
                    opt => opt.MapFrom(src => src.ExamQuestions != null ? src.ExamQuestions.Count : 0))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ForMember(dest => dest.ExamQuestions!, opt => opt.Ignore())
                .ForMember(dest => dest.NoOfQuestions, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RoomExamDto, RoomExam>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.Exam, opt => opt.MapFrom(src => src.Exams!.FirstOrDefault()))
                .ReverseMap()
                .ForMember(dest=>dest.Exams,opt=>opt.MapFrom(src=> src.Exam!=null?new List<Exam> { src.Exam }:null));

            CreateMap<ExamQuestionDto, ExamQuestion>().ReverseMap();
            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ForMember(dest => dest.Chapter, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionBank, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TeacherDto, Teacher>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ReverseMap();
            CreateMap<AnswerDto, Answer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ReverseMap();
            CreateMap<QuestionBankDto, QuestionBank>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ReverseMap();
            CreateMap<ChapterDto, Chapter>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ReverseMap();
            CreateMap<CourseClassDto, CourseClass>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ReverseMap();
            //CreateMap<QuestionsAddedToExamDto,Question>().ReverseMap();



            CreateMap<QuestionImportPreviewDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))

                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answer != null
                    ? src.Answer.Select(a => new Answer
                    {
                        Id = Guid.NewGuid(),
                        Content = a.Content,
                        IsCorrect = a.IsCorrect,

                    }).ToList()
                    : new List<Answer>()))
                .ForMember(dest => dest.Chapter, opt => opt.Ignore())
                .ForMember(dest => dest.ExamQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.StudentExamDetails, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionBank, opt => opt.Ignore());
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<CourseClass, CourseClassSearchDto>().ReverseMap();
            CreateMap<Exam, SearchExam>().ReverseMap();
            CreateMap<RoomExam, AddRoomExamDto>().ReverseMap()
                .ForMember(dest => dest.Subject, opt => opt.Ignore());


            CreateMap<UserEmailDto, Student>().ReverseMap();

            CreateMap<ChapterDto, Chapter>();
            CreateMap<CreateSubjectDto, Subject>()
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters));

            CreateMap<StudentExam, StudentExamDto>().ReverseMap();
            CreateMap<AddUserDtos, Student>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
               
            CreateMap<AddUserDtos, Teacher>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
               

            CreateMap<ChapterInfoDto, Chapter>().ReverseMap();


        }

    }
}
