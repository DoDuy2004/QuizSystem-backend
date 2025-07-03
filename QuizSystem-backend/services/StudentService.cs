using OfficeOpenXml;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Enums;
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

        //public async Task<bool> ImportStudentConfirm(List<StudentImportDto> studentsImportPreviewDto)
        //{
        //    var students = new List<Student>();
        //    foreach (var studentDto in studentsImportPreviewDto)
        //    {
        //        if (studentDto.IsValid)
        //        {
        //            var student = new Student
        //            {
        //                StudentCode = studentDto.StudentCode,
        //                Password = studentDto.Password,
        //                Email = studentDto.Email,
        //                FullName = studentDto.FullName,
        //                status = studentDto.status ?? Status.Active
        //            };
        //            students.Add(student);
        //        }
        //    }
        //    // Lưu danh sách sinh viên hợp lệ vào cơ sở dữ liệu
        //    // Giả sử bạn có một phương thức SaveStudentsAsync trong repository
        //    return await _studentRepository.SaveStudentsAsync(students);
        //}
        public async Task<List<StudentImportDto>> ImportFileStudentPreview(IFormFile file)
        {
            var listStudent = new List<StudentImportDto>();
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
                        var student = new StudentImportDto
                        {
                            StudentCode = worksheet.Cells[row, 1].Text.Trim(),
                            Password = worksheet.Cells[row, 2].Text.Trim(),
                            Email = worksheet.Cells[row, 3].Text.Trim(),
                            FullName = worksheet.Cells[row, 4].Text.Trim(),
                            status = Enum.TryParse<Status>(worksheet.Cells[row, 5].Text, out var status) ? status : null
                        };

                        student.ErrorMessages = new List<string>();
                        //Validate dữ liệu
                        if (string.IsNullOrEmpty(student.StudentCode))
                            student.ErrorMessages.Add("Mã sinh viên không được để trống.");
                        else if (student.StudentCode.Length != 10)
                            student.ErrorMessages.Add("Mã sinh viên phải có 10 ký tự.");

                        if (string.IsNullOrEmpty(student.Password))
                            student.ErrorMessages.Add("Mật khẩu không được để trống.");

                        if (string.IsNullOrEmpty(student.Email))
                            student.ErrorMessages.Add("Email không được để trống.");
                        else if (!student.Email.EndsWith("@caothang.edu.vn"))
                            student.ErrorMessages.Add("Email không hợp lệ.");

                        if (string.IsNullOrEmpty(student.FullName))
                            student.ErrorMessages.Add("Họ và tên không được để trống.");

                        if (student.status == null)
                            student.ErrorMessages.Add("Trạng thái không hợp lệ.");

                        student.IsValid = !student.ErrorMessages.Any();

                        listStudent.Add(student);
                    }
                    return listStudent;
                }
            }

        }
    }
}
