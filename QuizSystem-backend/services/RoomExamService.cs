using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OfficeOpenXml;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.RoomExamDtos;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.DTOs.UserDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services.MailServices;

namespace QuizSystem_backend.services;

public class RoomExamService : IRoomExamService
{
    private readonly IRoomExamRepository _roomExamRepository;
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;
    private readonly IExamRepository _examRepository;
    private readonly ICourseClassRepository _courseClassRepository;
    private readonly IEmailSender _mailService;

    public RoomExamService(IRoomExamRepository roomExamRepository, IMapper mapper, IStudentRepository studentRepository, IExamRepository examRepository, ICourseClassRepository courseClassRepository, IEmailSender mailService)
    {
        _roomExamRepository = roomExamRepository;
        _mapper = mapper;
        _studentRepository = studentRepository;
        _examRepository = examRepository;
        _courseClassRepository = courseClassRepository;
        _mailService = mailService;

    }
    public async Task<AddRoomExamResult> AddRoomExamAsync(AddRoomExamDto roomExamDto)
    {
        if (roomExamDto == null) return new AddRoomExamResult
        {
            Success = false,
            ErrorMessages = "Room exam data is null"
        };

        var exam = await _examRepository.GetExamByIdAsync(roomExamDto.ExamId);

        if (exam == null) return new AddRoomExamResult
        {
            Success = false,
            ErrorMessages = "Exam not found"
        };
        var courseClass = await _courseClassRepository.GetByIdAsync(roomExamDto.CourseClassId);

        if (courseClass == null) return new AddRoomExamResult
        {
            Success = false,
            ErrorMessages = "Course class not found"
        };

        //string> mails,string subject,string htmlMessage
        var listStudent = courseClass.Students.Select(s => s.Student).ToList();

        foreach (var s in courseClass.Students)
        {
            Console.WriteLine($"StudentId: {s.Student?.Id}, FullName: {s.Student?.FullName}, Email: {s.Student?.Email}");
        }

        Console.WriteLine(listStudent);

        var listUserEmail = _mapper.Map<List<UserEmailDto>>(listStudent);

        var listEmail = listUserEmail.Select(u => u.Email).ToList();

        var roomExam = _mapper.Map<RoomExam>(roomExamDto);

        roomExam.ExamId = exam.Id;
        roomExam.Exam = exam;

        roomExam.CourseClassId = courseClass.Id;
        roomExam.SubjectId = courseClass.SubjectId;

        var addedRoomExam = await _roomExamRepository.AddAsync(roomExam);

        var result = _mapper.Map<AddRoomExamDto>(addedRoomExam);

        result.Exams = new List<Exam> { exam };

        result.SubjectName = courseClass.Subject.Name;

        result.CourseClassName = courseClass.Name;

        var mailContent = new MailContent();
        mailContent.Subject = "Thông báo tham gia kỳ thi mới trên EduQuiz";
        mailContent.Body = $@"Xin chào,

                            Bạn vừa được thêm vào kỳ thi ""{roomExam.Name}"" trên hệ thống EduQuiz.
                            Vui lòng đăng nhập vào hệ thống để kiểm tra thông tin kỳ thi và hoàn thành bài thi đúng thời gian quy định.

                            Chúc bạn làm bài thật tốt!
                            Trân trọng,
                            Đội ngũ EduQuiz";

        foreach (var mail in listEmail)
        {
            Task.Run(() => _mailService.SendEmailAsync(mail, mailContent.Subject, mailContent.Body));

        }

        return new AddRoomExamResult()
        {
            Success = true,
            RoomExam = result,
        };
    }

    public async Task<(bool Success, string? ErrorMessages, IEnumerable<ExamDto>? RoomExams)> GetListExamAsync(int limit, string key)
    {
        var exams = await _roomExamRepository.GetListExamAsync(limit, key);
        if (exams == null || !exams.Any())
        {
            return (false, "No exams found", null!);
        }
        var examDtos = _mapper.Map<IEnumerable<ExamDto>>(exams);
        return (true, null, examDtos);
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


}
