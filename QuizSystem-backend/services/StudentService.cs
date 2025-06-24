using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

namespace QuizSystem_backend.services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository) 
        {
            _studentRepository = studentRepository; 
        }
        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return students;
        }
        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            return student;
        }
    }
}
