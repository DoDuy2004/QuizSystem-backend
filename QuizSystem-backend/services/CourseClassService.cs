using Microsoft.EntityFrameworkCore.Metadata;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace QuizSystem_backend.services
{
    public class CourseClassService : ICourseClassService
    {
        private readonly ICourseClassRepository _courseClassRepository;
        private readonly IStudentRepository _studentRepository;

        public CourseClassService(ICourseClassRepository courseClassRepository, IStudentRepository studentRepository)
        {
            _courseClassRepository = courseClassRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<CourseClassDto>> GetCourseClassesAsync()
        {
            var courses = await _courseClassRepository.GetCourseClassesAsync();

            return courses.Select(c => new CourseClassDto(c));
        }
        public async Task<CourseClassDto> GetCourseClassByIdAsync(Guid id)
        {
            var course = await _courseClassRepository.GetByIdAsync(id);

            return new CourseClassDto(course);
        }
        public async Task<(bool success, string? message, object? data)> AddStudentToCourseAsync(StudentCourseClassDto scc)
        {
            var student = await _studentRepository.GetByIdAsync(scc.StudentId);
            var courseClass = await _courseClassRepository.GetByIdAsync(scc.CourseClassId);
            //var existed = 

            if (student == null)
            {
                return (false, $"Student with {scc.StudentId} not found", null!);
            }

            if(courseClass == null)
            {
                return (false, $"Course class with {scc.CourseClassId} not found", null!);
            }

            await _courseClassRepository.AddStudentToCourseAsync(new StudentCourseClass(scc));

            return (true, null, new StudentDto(student));
        }
        public async Task<IEnumerable<StudentDto>> GetStudentByCourseClassAsync(Guid id)
        {
            var students = await _courseClassRepository.GetStudentByCourseClassAsync(id);

            return students.Select(s => new StudentDto(s));
        }
        public async Task<bool> DeleteCourseClassAsync(Guid id)
        {
            var courseClass = await _courseClassRepository.GetByIdAsync(id);

            if (courseClass.Status == Status.DELETED || courseClass == null)
            {
                return false;
            }

            courseClass.Status = Status.DELETED;

            await _courseClassRepository.SaveChangesAsync();

            return true;
        }
        public async Task<CourseClassDto> AddCourseClassAsync(CourseClassDto dto)
        {
            var courseClass = await _courseClassRepository.AddAsync(new CourseClass(dto));

            return new CourseClassDto(courseClass);
        }
        public async Task<CourseClassDto> UpdateCourseClassAsync(Guid id, CourseClassDto dto)
        {
            var updatedCourseClass = await _courseClassRepository.GetByIdAsync(id);

            if(updatedCourseClass == null)
            {
                return null!;
            }

            updatedCourseClass.ClassCode = dto.ClassCode;
            updatedCourseClass.Name = dto.Name;
            updatedCourseClass.Credit = dto.Credit;
            updatedCourseClass.Status = dto.Status;
            updatedCourseClass.TeacherId = dto.TeacherId;
            updatedCourseClass.Subject = dto.Subject;

            await _courseClassRepository.SaveChangesAsync();

            return new CourseClassDto(updatedCourseClass);
        }
    }
}
