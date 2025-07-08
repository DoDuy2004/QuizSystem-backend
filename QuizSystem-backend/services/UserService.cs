using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.UserDtos;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

namespace QuizSystem_backend.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task SaveChangeAsync()
        {

        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            return user;
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            return user;
        }


        public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var result = _userRepository.CheckPasswordAsync(user, oldPassword);

            if (!result) return false;

            await _userRepository.ChangePasswordAsync(user, newPassword);

            return true;
        }

        public async Task<(bool Succeed, string Message)> AddUser(AddUserDtos userDto)
        {
            var userExist = await _userRepository.GetByUsernameAsync(userDto.Email);

            if (userExist != null) return (false, "Student đã tồn tại");

            if (Enum.TryParse(userDto.Role, out Enums.Role role) && role == Enums.Role.STUDENT)
            {
                var student = _mapper.Map<Student>(userDto);
                student.StudentCode = userDto.Code;
                var studentHasher = new PasswordHasher<Student>();
                student.PasswordHash = studentHasher.HashPassword(student, userDto.Password);
                await _userRepository.AddSingle(student);
                return (true, "Tạo tài khoản thành công");
            }


            var teacher = _mapper.Map<Teacher>(userDto);
            teacher.TeacherCode = userDto.Code;
            var teacherHasher = new PasswordHasher<Teacher>();
            teacher.PasswordHash = teacherHasher.HashPassword(teacher, userDto.Password);
            await _userRepository.AddSingle(teacher);
            return (true, "Tạo tài khoản thành công");

        }

    }
}
