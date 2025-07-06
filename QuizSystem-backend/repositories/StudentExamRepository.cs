using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class StudentExamRepository:IStudentExamRepository
    {
        private readonly QuizSystemDbContext _context;
        private readonly IMapper _mapper;

        public StudentExamRepository(QuizSystemDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper= mapper;
        }
   
        public async Task<StudentExam?>AddStudentExamAsync(StudentExam studentExam)
        {
            _context.StudentExams.Add(studentExam);
            await _context.SaveChangesAsync();
            return studentExam;
        }
        public async Task<StudentExamResultDto?> GradeStudentExamAsync(Guid studentExamId)
        {
            var studentExam = await _context.StudentExams
                .Include(se => se.StudentExamDetails)
                .FirstOrDefaultAsync(se => se.Id == studentExamId);

            if (studentExam == null) throw new Exception("Bài làm không tồn tại!");


            var examId = studentExam.ExamId;

            var exam = await _context.Exams
                                    .Include(e => e.ExamQuestions)
                                        .ThenInclude(eq => eq.Question)
                                            .ThenInclude(q => q.Answers)
                                    .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return null;

            var questions = exam.ExamQuestions.Select(eq => eq.Question).ToList();

            int totalQuestion = questions.Count;

            int correctCount = 0;

            var questionResults= new List<QuestionResultDto>();

            foreach (var question in questions)
            {
                var studentAnswers = studentExam.StudentExamDetails
                    .Where(d => d.QuestionId == question.Id)
                    .Select(d => d.AnswerId)
                    .ToList();

                var correctAnswers = question.Answers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id)
                    .ToList();

                bool isCorrect = false;
                switch (question.Type) 
                {
                    case TypeOfQuestion.SingleChoice:
                    case TypeOfQuestion.TrueFalse:
                        isCorrect = studentAnswers.Count == 1 && correctAnswers.Contains(studentAnswers.First());
                        break;
                    case TypeOfQuestion.MultipleChoice:
                        isCorrect = studentAnswers.Count == correctAnswers.Count && studentAnswers.All(correctAnswers.Contains);

                        break;
                }

                if (isCorrect) correctCount++;

                questionResults.Add(new QuestionResultDto
                {
                    QuestionId = question.Id,
                    Content = question.Content,
                    //Type = question.Type,
                    CorrectAnswerIds = correctAnswers,
                    StudentAnswerIds = studentAnswers,
                    IsCorrect = isCorrect
                });
            }

            float grade = (float)correctCount / totalQuestion * 10; 

            studentExam.Grade = grade;
            await _context.SaveChangesAsync();
            var studentExamResult = new StudentExamResultDto
            {
                Grade = grade,
                TotalQuestion = totalQuestion,
                CorrectCount = correctCount,
                QuestionResults = questionResults
            };

            return studentExamResult;
        }
        public async Task<List<StudentExamDto>>GetListStudentExamAsync(Guid studentId)
        {
            var listStudentExam=await _context.StudentExams.Where(s=>s.StudentId==studentId).ToListAsync();

            return  _mapper.Map<List<StudentExamDto>>(listStudentExam);
            
        }
    }
}
