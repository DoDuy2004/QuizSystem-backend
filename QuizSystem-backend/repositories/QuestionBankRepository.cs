using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class QuestionBankRepository : IQuestionBankRepository
    {
        private readonly QuizSystemDbContext _context;
        public QuestionBankRepository(QuizSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<QuestionBank>> GetQuestionBanksAsync()
        {
            var questionBanks = await _context.QuestionBanks
                .Include(qb => qb.Questions)
                    .ThenInclude(q => q.Teacher)
                .Include(qb => qb.Questions)
                    .ThenInclude(q => q.Chapter)
                .Include(qb => qb.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(qb => qb.Course)
                .ToListAsync();

            return questionBanks;
        }
    }
}
