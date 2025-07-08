using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.DTOs.TeacherDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

namespace QuizSystem_backend.services
{
    public class TeacherService:ITeacherService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeacherRepository _teacherRepository;
        public TeacherService(IUserRepository userRepository,ITeacherRepository teacherRepository)
        {
            _userRepository= userRepository;
            _teacherRepository= teacherRepository;
        }

        public Task<NotificationForCourseClass> AddNotificationAsync(NotificationForCourseClass notification)
        {
            // Logic to add a notification for a course class
            // This would typically involve saving the notification to a database
            throw new NotImplementedException("Method not implemented yet.");
        }
        public async Task<List<Teacher>> ImportTeacherConfirm(List<TeacherImportDto> teachersPreview)
        {

            var teachers = new List<Teacher>();
            var hasher = new PasswordHasher<User>();

            if (teachersPreview == null || !teachersPreview.Any())
            {
                return teachers;
            }

            foreach (var teacherDto in teachersPreview)
            {
                if (!teacherDto.IsValid)
                    continue;

                var teacher = new Teacher
                {
                    Id = Guid.NewGuid(),
                    TeacherCode = teacherDto.TeacherCode!,
                    Email = teacherDto.Email!,
                    FullName = teacherDto.FullName!,
                    Status = teacherDto.status ?? Status.ACTIVE,
                    Username = teacherDto.Email!,
                    Role = Role.TEACHER,
                    CreatedAt=DateTime.UtcNow,

                };

                teacher.PasswordHash = hasher.HashPassword(teacher, teacherDto.Password!);
                teachers.Add(teacher);
            }
            await _teacherRepository.SaveTeachersAsync(teachers);

            return teachers;
        }
        public async Task<List<TeacherImportDto>> ImportFileTeacherPreview(IFormFile file)
        {
            var listTeacher = new List<TeacherImportDto>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                // Đặt ở đầu code dùng EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var teacher = new TeacherImportDto
                        {
                            TeacherCode = worksheet.Cells[row, 1].Text.Trim(),
                            Password = worksheet.Cells[row, 2].Text.Trim(),
                            Email = worksheet.Cells[row, 3].Text.Trim(),
                            FullName = worksheet.Cells[row, 4].Text.Trim(),
                            status = Enum.TryParse<Status>(worksheet.Cells[row, 5].Text, out var status) ? status : null
                        };

                        var existingTeacher = await _teacherRepository.GetAllTeacherAsync();

                        teacher.ErrorMessages = new List<string>();

                        //Validate dữ liệu
                        if (string.IsNullOrEmpty(teacher.TeacherCode))
                            teacher.ErrorMessages.Add("Mã giáo viên không được để trống.");

                        else if (existingTeacher.Any(s => s.TeacherCode == teacher.TeacherCode))
                        {
                            teacher.ErrorMessages.Add("Mã giáo viên đã được thêm .");
                        }

                        if (string.IsNullOrEmpty(teacher.Password))
                            teacher.ErrorMessages.Add("Mật khẩu không được để trống.");

                        if (string.IsNullOrEmpty(teacher.Email))
                            teacher.ErrorMessages.Add("Email không được để trống.");

                        else if (!teacher.Email.EndsWith("@caothang.edu.vn"))
                            teacher.ErrorMessages.Add("Email không hợp lệ.");

                        else if (existingTeacher.Any(s => s.Email == teacher.Email))
                        {
                            teacher.ErrorMessages.Add("Email đã tồn tại.");
                            teacher.IsValid = false;
                        }

                        if (string.IsNullOrEmpty(teacher.FullName))
                            teacher.ErrorMessages.Add("Họ và tên không được để trống.");

                        if (teacher.status == null)
                            teacher.ErrorMessages.Add("Trạng thái không hợp lệ.");

                        teacher.IsValid = !teacher.ErrorMessages.Any();

                        listTeacher.Add(teacher);
                    }
                    var duplicateEmails = listTeacher
                        .GroupBy(x => x.Email)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();
                    var duplicateteacherCodes = listTeacher
                        .GroupBy(x => x.TeacherCode)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();
                    foreach (var teacher in listTeacher)
                    {
                        if (duplicateEmails.Contains(teacher.Email))
                        {
                            teacher.ErrorMessages.Add("Email đã tồn tại trong danh sách.");
                            teacher.IsValid = false;
                        }
                        if (duplicateteacherCodes.Contains(teacher.TeacherCode))
                        {
                            teacher.ErrorMessages.Add("Mã sinh viên đã tồn tại trong danh sách.");
                            teacher.IsValid = false;
                        }
                    }
                    return listTeacher;
                }
            }

        }

    }
}