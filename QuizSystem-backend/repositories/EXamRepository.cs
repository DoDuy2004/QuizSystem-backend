using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using static QuizSystem_backend.DTOs.ExamDto;

namespace QuizSystem_backend.repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly QuizSystemDbContext _context;
        public ExamRepository(QuizSystemDbContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<Exam>> GetExamsAsync()
        {
            return await _context.Exams
                .AsNoTracking()
                //.Include(e => e.RoomExam)
                .Include(e => e.Subject)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                //        .ThenInclude(q => q.QuestionBank)
                //.Include(e => e.ExamQuestions) 
                //    .ThenInclude(eq => eq.Question)
                //        .ThenInclude(q => q.Answers) 
                .ToListAsync();
        }


        public async Task<Exam> GetExamByIdAsync(Guid id)
        {
            var exam = await _context.Exams
                //.Include(e => e.RoomExam)
                .Include(e => e.Subject)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                //        .ThenInclude(q => q.QuestionBank)
                //.Include(e => e.ExamQuestions!)
                //    .ThenInclude(eq => eq.Question)
                //        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == id);
            return exam!;
        }

        public async Task<List<Question>?> GetQuestionsByExamAsync(Guid id)
        {
            var q= await _context.ExamQuestions
                .Where(eq => eq.ExamId == id)
                .Include(eq => eq.Question)
                    .ThenInclude(q => q.Answers)
                .ToListAsync();

            List<Question> list = new();
            foreach (var questionExam in q) list.Add(questionExam.Question);
            return list;
                
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetQuestionsByChapterAndDifficultyAsync(Guid chapterId, Difficulty difficulty, int take)
        {
            return await _context.Questions
                .Where(q => q.ChapterId == chapterId && q.Difficulty == difficulty)
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

        public async Task<Question> AddQuestionToExamAsync(Guid examId, Question question, float score)
        {
            // Kiểm tra exam có tồn tại không
            var exam = await _context.Exams
                .Include(e => e.ExamQuestions)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null)
            {
                throw new InvalidOperationException("Exam not found.");
            }

            var existingQuestion = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == question.Id || q.Content == question.Content);

            if (existingQuestion == null)
            {
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                existingQuestion = question;
            }

            if (exam.ExamQuestions.Any(eq => eq.QuestionId == existingQuestion.Id))
            {
                throw new InvalidOperationException("This question is already in the exam.");
            }

            var examQuestion = new ExamQuestion
            {
                ExamId = examId,
                QuestionId = existingQuestion.Id,
                Score = score,
            };

            _context.ExamQuestions.Add(examQuestion);
            await _context.SaveChangesAsync();

            return existingQuestion;
        }

        public async Task<bool> DeleteQuestionFromExamAsync(Guid examId, Guid questionId)
        {
            var examQuestion = await _context.ExamQuestions
                .FirstOrDefaultAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);

            if (examQuestion == null)
                return false;

            _context.ExamQuestions.Remove(examQuestion);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<List<Exam>> GetListExamAsync(int limit, string key)
        {
            var list = _context.Exams.Where(e => e.Name.Contains(key))
                .Take(limit)
                .ToListAsync();
            return list;
        }
    }
}
