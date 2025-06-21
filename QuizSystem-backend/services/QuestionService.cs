using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

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
            var questions = await _questionRepository.GetQuestions();

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

            return new QuestionDto(newQuestion);
        } 

        public async Task<QuestionDto> UpdateQuestionAsync(Guid id, QuestionDto dto)
        {
            var updatedQuestion = await _questionRepository.GetByIdAsync(id);

            if (updatedQuestion == null)
            {
                return null!;
            }

            updatedQuestion.Content = dto.Content;
            updatedQuestion.Image = dto.Image!;
            updatedQuestion.Difficulty = dto.Difficulty;
            updatedQuestion.CreatedBy = dto.Teacher.Id;
            updatedQuestion.ChapterId = dto.Chapter.Id;
            updatedQuestion.Type = dto.Type;
            updatedQuestion.Topic = dto.Topic;
            updatedQuestion.QuestionBankId = dto.QuestionBank.Id;

            await _questionRepository.SaveChangesAsync();

            return new QuestionDto(updatedQuestion);
        }
    }
}
