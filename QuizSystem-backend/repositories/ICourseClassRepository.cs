﻿using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface ICourseClassRepository
    {
        Task<IEnumerable<CourseClass>> GetCourseClassesAsync();
        Task<IEnumerable<CourseClass>> GetCourseClassesByTeacherAsync(Guid userId);
        Task<IEnumerable<CourseClass>> GetCourseClassesByStudentAsync(Guid userId);
        Task<CourseClass> GetByIdAsync(Guid id);
        Task<CourseClass> AddAsync(CourseClass cc);
        Task SaveChangesAsync();
        Task<Student> AddStudentToCourseAsync(StudentCourseClass scc);
        Task<IEnumerable<Student>> GetStudentByCourseClassAsync(Guid id);
        Task<User> GetTeacherByCourseClassAsync(Guid id);
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<List<CourseClass>> GetListCourseClassAsync(int limit, string key);


        Task<Student?> DeleteStudentFromCourseClass(StudentCourseClass scc);
        Task<StudentCourseClass?> GetStudentCourseClassAsync(Guid studentId, Guid courseClassId);
        
    }
}
