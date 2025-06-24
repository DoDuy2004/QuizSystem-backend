using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using System.Net.WebSockets;

namespace QuizSystem_backend.services
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IQuestionBankRepository _questionBankRepository;
        public QuestionBankService(IQuestionBankRepository questionBankRepository)
        {
            _questionBankRepository = questionBankRepository;
        }
        public async Task<IEnumerable<QuestionBankDto>> GetQuestionBanksAsync()
        {
            var questionBanks = await _questionBankRepository.GetQuestionBanksAsync();

            return questionBanks.Select(qb => new QuestionBankDto(qb));
        }
        public async Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);

            return new QuestionBankDto(questionBank);
        }

        public async Task<QuestionBankDto> AddQuestionBankAsync(QuestionBankDto dto)
        {
            var questionBank = new QuestionBank(dto);

            await _questionBankRepository.AddAsync(questionBank);

            var newQuestionBank = await _questionBankRepository.GetQuestionBankByIdAsync(questionBank.Id);

            return new QuestionBankDto(newQuestionBank);
        }

        public async Task<QuestionBankDto> UpdateQuestionBankAsync(Guid id, QuestionBankDto dto)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);

            questionBank.Description = dto.Description!;
            questionBank.Name = dto.Name;
            questionBank.Status = dto.Status;
            questionBank.CourseClassId = dto.CourseClassId;

            await _questionBankRepository.SaveChangesAsync();

            return new QuestionBankDto(questionBank);
        }
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByQuestionBankAsync(Guid id)
        {
            var questions = await _questionBankRepository.GetQuestionsByQuestionBankAsync(id);

            return questions.Select(q => new QuestionDto(q));
        }
        public async Task<bool> DeleteQuestionBankAsync(Guid id)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);
             
            if (questionBank.Status == Status.DELETED || questionBank == null)
            {
                return false;
            }

            questionBank.Status = Status.DELETED;

            await _questionBankRepository.SaveChangesAsync();

            return true;
        }
    }
}
