using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.repositories;
using System.Diagnostics;

namespace QuizSystem_backend.services
{
    public class StudentExamService : IStudentExamService
    {
        private readonly IStudentExamRepository _studentExamRepository;

        public StudentExamService(IStudentExamRepository studentExamRepository)
        {
            _studentExamRepository = studentExamRepository;
        }
        public async Task<StudentExamResultDto>GetStudentExamResult(Guid studentExamId)
        {
            if (studentExamId == Guid.Empty)
            {
                throw new ArgumentException("Student exam ID cannot be empty.", nameof(studentExamId));
            }
            var result= await _studentExamRepository.GradeStudentExamAsync(studentExamId);

            
            
            return result!;
        }




    }
}
