using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizSystemDbContext _context;
        public QuestionRepository(QuizSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            var result = await _context.Questions
                .Include(q => q.Teacher)
                .Include(q => q.Chapter)
                    .ThenInclude(c => c.Course)
                .Include(q => q.QuestionBank)
                    .ThenInclude(qb => qb.Teacher)
                .Include(q => q.Answers)
                .ToListAsync();

            return result;
        }

        public async Task<Question> AddAsync(Question question)
        {
            _context.Questions.Add(question);

            await _context.SaveChangesAsync();

            return question;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetByIdAsync(Guid id)
        {
            var question = await _context.Questions
                .Include(q => q.Teacher)
                .Include(q => q.Chapter)
                    .ThenInclude(c => c.Course)
                .Include(q => q.QuestionBank)
                    .ThenInclude(qb => qb.Teacher)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            return question!;
        }
    }
}
