using QuizSystem_backend.DTOs;
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
    }
}
