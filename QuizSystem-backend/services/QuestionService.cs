using AutoMapper;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using System.Formats.Asn1;

namespace QuizSystem_backend.services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
      
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync()
        {
            var questions = await _questionRepository.GetQuestionsAsync();

            return questions.Select(q => new QuestionDto(q));
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            return new QuestionDto(question);
        }

        public async Task<QuestionDto> AddQuestionAsync(QuestionDto dto)
        {
            var newQuestion = new Question(dto);

            await _questionRepository.AddAsync(newQuestion);

            var inserted = await _questionRepository.GetByIdAsync(newQuestion.Id);

            return new QuestionDto(inserted);
        } 

        public async Task<QuestionDto> UpdateQuestionAsync(Guid id, QuestionDto dto)
        {
            var updatedQuestion = await _questionRepository.GetByIdAsync(id);
            var chapterId = dto.Chapter != null ? dto.Chapter.Id : dto.ChapterId;
            var createdBy = dto.Teacher != null ? dto.Teacher.Id : dto.CreatedBy;
            var questionBankId = dto.QuestionBank != null ? dto.QuestionBank.Id : dto.QuestionBankId;

            if (updatedQuestion == null)
            {
                return null!;
            }

            updatedQuestion.Content = dto.Content;
            updatedQuestion.Image = dto.Image!;
            updatedQuestion.Difficulty = dto.Difficulty;
            updatedQuestion.CreatedBy = createdBy;
            updatedQuestion.ChapterId = chapterId;
            updatedQuestion.Type = dto.Type;
            updatedQuestion.Topic = dto.Topic;
            updatedQuestion.QuestionBankId = questionBankId;
            updatedQuestion.Answers = dto.Answers!.Select(a => new Answer
            {
                Id = a.Id,
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                AnswerOrder = a.AnswerOrder,
                QuestionId = dto.Id,
            }).ToList();

            await _questionRepository.SaveChangesAsync();

            return new QuestionDto(updatedQuestion);
        }

        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null || question.Status == Status.DELETED)
                return false;

            question.Status = Status.DELETED;

            await _questionRepository.SaveChangesAsync();

            return true;
        }

        
    }
}
