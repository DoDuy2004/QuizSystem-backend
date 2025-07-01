using Azure.Core;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using static QuizSystem_backend.DTOs.ExamDto;

namespace QuizSystem_backend.services
{
    public interface IExamServices
    {
        Task<IEnumerable<ExamDto>> GetExamsAsync();
        Task<ExamDto> GetExamByIdAsync(Guid id);
        Task<ExamDto> AddExamAsync(ExamDto examDto);
        Task<bool> DeleteExamAsync(Guid id);
        Task<ExamDto> UpdateExamAsync(Guid id, ExamDto examDto);
        Task<List<QuestionDto>?> GetAllQuestionOfExam(Guid examId);
        Task<ExamDto> CreateExamByMatrixAsync(ExamMatrixRequest request,Guid questionBankId);
        Task<AddListQuestionDto> AddListQuestionToExamAsync(AddListQuestionDto dto);
        Task<bool> DeleteQuestionFromExamAsync(Guid examId, Guid questionId);

    }
}
