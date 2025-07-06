using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.AnswerDtos;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.QuestionDtos;
using QuizSystem_backend.DTOs.ResultInfoDto;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using static QuizSystem_backend.DTOs.ExamDto;

namespace QuizSystem_backend.services
{
    public class ExamServices : IExamServices
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public ExamServices(IExamRepository examRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        public async Task<AddListQuestionDto> AddListQuestionToExamAsync(AddListQuestionDto dto)
        {
            var examId = dto.examId;

            
            if (dto.QuestionScores == null || !dto.QuestionScores.Any())
            {
                throw new ArgumentException("Question scores cannot be null or empty.", nameof(dto.QuestionScores));
            }
            
            foreach (var questionScore in dto.QuestionScores)
            {
                var question = _mapper.Map<Question>(questionScore.Question);


                var addedQuestion=await _examRepository.AddQuestionToExamAsync(examId, question,questionScore.Score);
            }
            await _examRepository.SaveChangesAsync();

            return dto;
        }
        public async Task<IEnumerable<ExamDto>> GetExamsAsync()
        {
            var exams = await _examRepository.GetExamsAsync();
            var examDto= _mapper.Map<IEnumerable<ExamDto>>(exams);
            return examDto;
        }
        public async Task<ExamDto> GetExamByIdAsync(Guid id)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null) return null!;
            return _mapper.Map<ExamDto>(exam);
        }
        public async Task<ExamDto> AddExamAsync(ExamDto examDto)
        {
            if (examDto == null) return null!;

            var exam = _mapper.Map<Exam>(examDto);
            var addedExam = await _examRepository.GenerateAsync(exam);

            return _mapper.Map<ExamDto>(addedExam);
        }

        
        public async Task<bool> DeleteExamAsync(Guid id)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null) return false;

            exam.Status = Status.DELETED;
            await _examRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ExamDto> UpdateExamAsync(Guid id, ExamDto examDto)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null) return null!;

            exam.Name = examDto.Name;
            exam.DurationMinutes = examDto.DurationMinutes;
            exam.Status = examDto.Status;
            exam.ExamCode = examDto.ExamCode;
            exam.SubjectId = examDto.SubjectId;

            //var examUpdated = await _examRepository.UpdateExamAsync(examUpdate);
            await _examRepository.SaveChangesAsync();

            return _mapper.Map<ExamDto>(exam);
        }



        public async Task<CreateMatrixResult> CreateExamByMatrixAsync(ExamMatrixRequest request)
        {
            var result = new CreateMatrixResult();

            var exam=await _examRepository.GetExamByIdAsync(request.ExamId);
            if(exam == null)
            {
                result.Success = false;
                result.ErrorMessages = "Exam not found";
                return result;
            }
            

            foreach (var row in request.Matrix)
            {
                foreach (var pair in row.DifficultyMap)
                {
                    var difficulty = pair.Key;
                    var count = pair.Value;

                    var questions = await _examRepository
                        .GetQuestionsByChapterAndDifficultyAsync(row.ChapterId, difficulty, count);

                    if (questions.Count < count)
                    {
                        result.ErrorMessages += $"Không đủ câu hỏi ở chương {row.ChapterId} độ khó {difficulty}. ";
                        return result;
                    }

                    foreach (var question in questions)
                    {
                        exam.ExamQuestions.Add(new ExamQuestion
                        {
                            Exam = exam,
                            QuestionId = question.Id,
                            Question = question
                        });
                    }
                }
            }

            await _examRepository.SaveChangesAsync();

            result.Success = true;
            result.Exam = _mapper.Map<ExamDto>(exam);
            result.ErrorMessages = "Tạo đề thi thành công";
            return result;
        }


        public async Task<List<QuestionDto>?>GetAllQuestionOfExam(Guid examId)
        {
            var listQuestion = await _examRepository.GetQuestionsByExamAsync(examId);
            var result= _mapper.Map<List<QuestionDto>>(listQuestion);
            return result;
        }

        public async Task<bool> DeleteQuestionFromExamAsync(Guid examId, Guid questionId)
        {
            var result = await _examRepository.DeleteQuestionFromExamAsync(examId, questionId);

            return result;
        }

        public async Task<List<SearchExam>>SearchExam(string key, int limit)
        {
            var exams = await _examRepository.GetListExamAsync(limit,key);

            var searchExam = _mapper.Map<List<SearchExam>>(exams);

            return searchExam;
        }

        public async Task<ExamForStudentDto> GetExamForStudentAsync(Guid examId)
        {
            if (examId == Guid.Empty) return null!;
            
            var exam=await _examRepository.GetExamByIdAsync(examId);

            if (exam == null) return null!;

            var examDto=new ExamForStudentDto()
            {
                Id = exam.Id,
                Name = exam.Name,
                DurationMinutes = exam.DurationMinutes,
                ExamCode = exam.ExamCode,
                NoOfQuestions = exam.ExamQuestions.Count,
                Questions = exam.ExamQuestions.Select(eq => new QuestionForStudentDto
                {
                    Id = eq.Question.Id,
                    Content = eq.Question.Content,
                    Answers = eq.Question.Answers.Select(a => new DTOs.AnswerDtos.AnswerForStudentDto
                    {
                        Id = a.Id,
                        Content = a.Content,
                    }).ToList()
                }).ToList()
            };

            return examDto;
        }

    }
}
