using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
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

        public async Task<List<QuestionDto>> AddListQuestionToExamAsync(AddListQuestionDto dto)
        {
            var examId = dto.examId;

            
            if (dto.QuestionScores == null || !dto.QuestionScores.Any())
            {
                throw new ArgumentException("Question scores cannot be null or empty.", nameof(dto.QuestionScores));
            }
            
            List<QuestionDto> questions = new List<QuestionDto>();
            foreach (var questionScore in dto.QuestionScores)
            {
                var question = _mapper.Map<Question>(questionScore.Question);

                var addedQuestion=await _examRepository.AddQuestionToExamAsync(examId, question,questionScore.Score);
                questions.Add(_mapper.Map<QuestionDto>(addedQuestion));
            }
            return questions;
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

        public async Task<ExamDto?> UpdateExamAsync(Guid id, ExamDto examDto)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null) return null;

            exam.Name = examDto.Name;
            exam.DurationMinutes = examDto.DurationMinutes;
            exam.Status = examDto.Status;
            exam.StartDate = examDto.StartDate;
           
            exam.ExamCode = examDto.ExamCode;
            exam.NoOfQuestions = examDto.NoOfQuestions;

            // Xóa toàn bộ câu hỏi cũ và thêm lại (hoặc dùng so sánh/phân biệt nếu cần cập nhật tinh vi hơn)
            exam.ExamQuestions = examDto.ExamQuestions.Select(eq => new ExamQuestion
            {
                ExamId = exam.Id,
                QuestionId = eq.QuestionId,
                Score = eq.Score,
                Order = eq.Order
            }).ToList();

            await _examRepository.SaveChangesAsync();

            return _mapper.Map<ExamDto>(exam);
        }


        public async Task<ExamDto> CreateExamByMatrixAsync(ExamMatrixRequest request,Guid questionBankId)
        {
            var exam = _mapper.Map<Exam>(request.Exam);

            foreach (var row in request.Matrix)
            {
                foreach (var pair in row.DifficultyMap)
                {
                    var difficulty = pair.Key;
                    var count = pair.Value;

                    var questions = await _examRepository
                        .GetQuestionsByChapterAndDifficultyAsync(row.ChapterId, difficulty, count,questionBankId);

                    if (questions.Count < count)
                        throw new Exception($"Không đủ câu hỏi ở Chương {row.ChapterId} mức độ {difficulty}");

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

            await _examRepository.GenerateAsync(exam);
            await _examRepository.SaveChangesAsync();

            return _mapper.Map<ExamDto>(exam);
        }



    }
}
