using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
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
                .Include(qb => qb.Course)
                .Include(qb => qb.Questions)
                .ToListAsync();

            return questionBanks;
        }

        public async Task<QuestionBank> GetQuestionBankByIdAsync(Guid id)
        {
            var questionBank = await _context.QuestionBanks
                .Include(qb => qb.Course)
                .Include(qb => qb.Questions)
                .FirstOrDefaultAsync(q => q.Id == id); ;

            return questionBank!;
        }
        public async Task<QuestionBank> AddAsync(QuestionBank questionBank)
        {
            _context.QuestionBanks.Add(questionBank);

            await _context.SaveChangesAsync();

            return questionBank;
        }

        public async Task<IEnumerable<Question>> GetQuestionsByQuestionBankAsync(Guid id)
        {
            var questions = await _context.Questions
                .Include(q => q.Teacher)
                .Include(q => q.Chapter)
                .Include(q => q.QuestionBank).ThenInclude(qb => qb.Course)
                .Include(q => q.Answers)
                .Where(q => q.QuestionBank.Id == id)
                .ToListAsync();

            return questions;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
