using AutoMapper;
using Microsoft.DotNet.Scaffolding.Shared;
using OfficeOpenXml;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

namespace QuizSystem_backend.services
{
    public class RoomExamService: IRoomExamService
    {
        private readonly IRoomExamRepository _roomExamRepository;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public RoomExamService(IRoomExamRepository roomExamRepository,IMapper mapper,IStudentRepository studentRepository)
        {
            _roomExamRepository = roomExamRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
          
        }
        public async Task<RoomExamDto> AddRoomExamAsync(RoomExamDto roomExamDto)
        {
            if (roomExamDto == null) return null!;

            var roomExam = _mapper.Map<RoomExam>(roomExamDto);

            var addedRoomExam = await _roomExamRepository.AddAsync(roomExam);

            return _mapper.Map<RoomExamDto>(addedRoomExam);
        }

        public async Task<IEnumerable<RoomExamDto>> GetAllRoomExamsAsync()
        {
            var roomExams = await _roomExamRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomExamDto>>(roomExams);
        }

        public async Task<RoomExamDto> GetRoomExamByIdAsync(Guid id)
        {
            var roomExam = await _roomExamRepository.GetByIdAsync(id);
            if (roomExam == null) return null!;

            return _mapper.Map<RoomExamDto>(roomExam);
        }
        public async Task<bool> DeleteRoomExamAsync(Guid id)
        {
            return await _roomExamRepository.DeleteAsync(id);
        }
        public async Task<bool> UpdateRoomExamAsync(RoomExamDto roomExamDto)
        {
            if (roomExamDto == null) return false;
            var roomExam = _mapper.Map<RoomExam>(roomExamDto);
            return await _roomExamRepository.UpdateAsync(roomExam);
        }
        public async Task SaveChangesAsync()
        {
            await _roomExamRepository.SaveChangesAsync();
        }

        public async Task<List<StudentImportDto>> ImportStudenInRoomExam(IFormFile file, Guid roomExamId)
        {
            var listStudent = new List<StudentImportDto>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    // Lấy room trước khi lặp để tránh gọi lại nhiều lần
                    var room = await _roomExamRepository.GetByIdAsync(roomExamId);

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

                        var isExistInRoom = room!.Students.Any(s => s.Student.Email == student.Email);

                        student.ErrorMessages = new List<string>();

                        //Validate dữ liệu
                        if (string.IsNullOrEmpty(student.StudentCode))
                            student.ErrorMessages.Add("Mã sinh viên không được để trống.");
                        else if (isExistInRoom)
                            student.ErrorMessages.Add("Mã sinh viên đã được thêm trước đó.");

                        if (string.IsNullOrEmpty(student.Password))
                            student.ErrorMessages.Add("Mật khẩu không được để trống.");

                        if (string.IsNullOrEmpty(student.Email))
                            student.ErrorMessages.Add("Email không được để trống.");
                        else if (!student.Email.EndsWith("@caothang.edu.vn"))
                            student.ErrorMessages.Add("Email không hợp lệ.");
                        else if (isExistInRoom)
                        {
                            student.ErrorMessages.Add("Email đã tồn tại.");
                            student.IsValid = false;
                        }

                        if (string.IsNullOrEmpty(student.FullName))
                            student.ErrorMessages.Add("Họ và tên không được để trống.");

                        if (student.status == null)
                            student.ErrorMessages.Add("Trạng thái không hợp lệ.");

                        student.IsValid = !student.ErrorMessages.Any();

                        listStudent.Add(student);
                    }

                    // Thêm các sinh viên hợp lệ vào roomExam
                    var validStudents = listStudent.Where(s => s.IsValid).ToList();
                    foreach (var studentDto in validStudents)
                    {
                        // Giả sử có phương thức chuyển đổi StudentImportDto sang Student và thêm vào room
                        var studentEntity = _mapper.Map<Student>(studentDto);
                        // Nếu room.Students là ICollection<RoomExamStudent>
                        room.Students.Add(new StudentRoomExam
                        {
                            RoomId = roomExamId,
                            Student = studentEntity
                        });
                    }

                    await _roomExamRepository.UpdateAsync(room);
                    await _roomExamRepository.SaveChangesAsync();

                    return listStudent;
                }
            }
        }
    }
}
