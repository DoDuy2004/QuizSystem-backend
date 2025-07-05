using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using OfficeOpenXml;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.StudentDtos;
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
        private readonly IMapper _mapper;

        public CourseClassService(ICourseClassRepository courseClassRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            _courseClassRepository = courseClassRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseClassDto>> GetCourseClassesAsync(Guid userId, string role)
        {
            IEnumerable<CourseClass> courses = Enumerable.Empty<CourseClass>(); ;

            if (role == "ADMIN")
            {
                courses = await _courseClassRepository.GetCourseClassesAsync();
            }
            else if (role == "TEACHER")
            {
                courses = await _courseClassRepository.GetCourseClassesByTeacherAsync(userId);
            }

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

            if (courseClass == null)
            {
                return (false, $"Course class with {scc.CourseClassId} not found", null!);
            }

            await _courseClassRepository.AddStudentToCourseAsync(new StudentCourseClass(scc));

            return (true, null, new StudentDto(student));
        }

        public async Task<(bool success, string? message, object? data)> AddListStudentToCourseAsync(List<StudentCourseClassDto> listScc)
        {

            if (listScc == null || listScc.Count == 0)
            {
                return (false, "List of students is empty", null!);
            }
            var list=new List<StudentCourseClassDto>();
            foreach (var scc in listScc)
            {
                var student = await _studentRepository.GetByIdAsync(scc.StudentId);
                var courseClass = await _courseClassRepository.GetByIdAsync(scc.CourseClassId);
                //var existed = 

                if (student == null)
                {
                    return (false, $"Student with {scc.StudentId} not found", null!);
                }

                if (courseClass == null)
                {
                    return (false, $"Course class with {scc.CourseClassId} not found", null!);
                }

                await _courseClassRepository.AddStudentToCourseAsync(new StudentCourseClass(scc));
                list.Add(scc);
            }
            

            return (true, null,list);
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

            if (updatedCourseClass == null)
            {
                return null!;
            }

            updatedCourseClass.ClassCode = dto.ClassCode!;
            updatedCourseClass.Name = dto.Name;
            updatedCourseClass.Credit = dto.Credit;
            //updatedCourseClass.UserId = dto.TeacherId;
            updatedCourseClass.SubjectId = dto.SubjectId;
            updatedCourseClass.Description = dto.Description!;

            await _courseClassRepository.SaveChangesAsync();

            return new CourseClassDto(updatedCourseClass);
        }
        public async Task<IEnumerable<SubjectDto>> GetSubjectsAsync()
        {
            var subjects = await _courseClassRepository.GetSubjectsAsync();

            return subjects.Select(s => new SubjectDto(s));
        }

        public async Task<(bool success, string? message, object? data)> DeleteStudentFromCourseAsync(StudentCourseClassDto scc)
        {
            var student = await _courseClassRepository.GetStudentCourseClassAsync(scc.StudentId, scc.CourseClassId);
            if (student == null)
            {
                return (false, $"Student with {scc.StudentId} not found in course class {scc.CourseClassId}", null!);
            }

            await _courseClassRepository.DeleteStudentFromCourseClass(student);

            return (true, null, _mapper.Map<StudentDto>(student));
        }

       

    }
}