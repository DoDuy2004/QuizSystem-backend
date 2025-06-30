using AutoMapper;
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
        private readonly IMapper _mapper;

        public QuestionBankService(IQuestionBankRepository questionBankRepository,IMapper mapper)
        {
            _questionBankRepository = questionBankRepository;
            _mapper = mapper;
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

            //if (!string.IsNullOrEmpty(dto.Subject))
            //{
            //    var existingSubject = await _context.Subjects
            //        .FirstOrDefaultAsync(s => s.Name == dto.Subject);

            //    if (existingSubject != null)
            //    {
            //        qb.SubjectId = existingSubject.Id;
            //    }
            //    else
            //    {
            //        var newSubject = new Subject
            //        {
            //            Id = Guid.NewGuid(),
            //            Name = dto.Subject
            //        };
            //        _context.Subjects.Add(newSubject);
            //        await _context.SaveChangesAsync();

            //        qb.SubjectId = newSubject.Id;
            //    }
            //}
            //else if (dto.SubjectId.HasValue)
            //{
            //    qb.SubjectId = dto.SubjectId;
            //}

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
            questionBank.Subject = dto.Subject!;
            //questionBank.CourseClassId = dto.CourseClassId;

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
        public async Task<List<QuestionDto>> AddListQuestionsToQuestionBankAsync(Guid questionBankId, List<QuestionDto> listQuestionDto)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(questionBankId);
            if (questionBank == null)
            {
                throw new Exception("Question bank not found");
            }
            foreach(var questionDto in listQuestionDto)
            {
                var newQuestion = _mapper.Map<Question>(questionDto);
                newQuestion.QuestionBank = questionBank;
                await _questionBankRepository.AddQuestionAsync(newQuestion);
            }
            return listQuestionDto;
        }
    }
}
