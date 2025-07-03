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


        public async Task<List<Question>> AddListQuestionAsync(List<Question> listQuestion)
        {
            await _context.Questions.AddRangeAsync(listQuestion);
            await _context.SaveChangesAsync();
            return listQuestion;
        }
        public async Task<string?> CheckErrorSubChap(string nameSubject, string nameChapter)
        {
            if (string.IsNullOrEmpty(nameSubject) || string.IsNullOrEmpty(nameChapter))
            {   
                return "Tên môn học hoặc chương không được để trống";
            }
            var result = await _context.Subjects.Include(s => s.Chapters).FirstOrDefaultAsync(s => s.Name == nameSubject);
            if (result == null) return "Môn học không tồn tại";
            if (!result.Chapters.Any(c => c.Name == nameChapter))
            {
                return "Chương không có trong môn học";
            }
            return null;
        }
        public async Task<IEnumerable<QuestionBank>> GetQuestionBanksAsync()
        {
            var questionBanks = await _context.QuestionBanks
                //.Include(qb => qb.Course)
                .Include(qb => qb.Questions)
                .ToListAsync();

            return questionBanks;
        }

        public async Task<QuestionBank> GetQuestionBankByIdAsync(Guid id)
        {
            var questionBank = await _context.QuestionBanks
                //.Include(qb => qb.Course)
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
                    .ThenInclude(qb => qb.Subject)
                .Include(q => q.Answers)
                .Where(q => q.QuestionBank.Id == id)
                .ToListAsync();

            return questions;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddQuestionAsync(Question newQuestion)
        {
            await _context.Questions.AddAsync(newQuestion);
        }
    }
}
