using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using static QuizSystem_backend.DTOs.ExamDto;

namespace QuizSystem_backend.repositories
{
    public class EXamRepository : IExamRepository
    {
        private readonly QuizSystemDbContext _context;
        public EXamRepository(QuizSystemDbContext context)
        {
            _context = context;
        }


       
        public async Task<IEnumerable<Exam>> GetExamsAsync()
        {
            return await _context.Exams
                .AsNoTracking()
                .Include(e => e.RoomExam)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.QuestionBank)
                .Include(e => e.ExamQuestions) 
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Answers) 
                .ToListAsync();
        }


        public async Task<Exam?> GetExamByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _context.Exams
                .AsNoTracking()
                .Include(e => e.RoomExam)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.QuestionBank)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<ExamQuestion>> GetQuestionsByExamAsync(Guid id)
        {
            return await _context.ExamQuestions
                .Where(eq => eq.ExamId == id)
                .Include(eq => eq.Question)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetQuestionsByChapterAndDifficultyAsync(Guid chapterId, Difficulty difficulty, int take, Guid questionBankId)
        {
            return await _context.Questions
                .Where(q => q.QuestionBankId==questionBankId && q.ChapterId == chapterId && q.Difficulty == difficulty)
                .OrderBy(q => Guid.NewGuid()) // random
                .Take(take)
                .ToListAsync();
        }

        // Existing method, but with async suffix for consistency
        public async Task<Exam> GenerateAsync(Exam exam)
        {
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<Question> AddQuestionToExamAsync(Guid examId, Question question,float score)
        {
            // Find the exam
            var exam = await _context.Exams
                .Include(e => e.ExamQuestions)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null)
            {
                throw new InvalidOperationException("Exam not found.");
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var examQuestion = new ExamQuestion
            {
                ExamId = examId,
                QuestionId = question.Id,
                Score = score
            };

            _context.ExamQuestions.Add(examQuestion);
            await _context.SaveChangesAsync();

            return question;
        }
    }
}
