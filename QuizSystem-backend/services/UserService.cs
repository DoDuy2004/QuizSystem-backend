using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.AuthDto;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using System.Security.Claims;

namespace QuizSystem_backend.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInMangager;

        public UserService(IUserRepository userRepository,IMapper mapper,SignInManager<AppUser>signInManager,UserManager<AppUser>userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _signInMangager = signInManager;
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            return user;
        }
        public async Task<AppUser> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            return user;
        }
       
        public async Task<bool> Resgister(RegisterDto registerDto, Role role)
        {
            var user = _mapper.Map<AppUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return false;

            await _userManager.AddToRoleAsync(user, role.ToString());
            return true;
        }

        public async Task<bool> GrantPermission(Guid userId, string claimString, string value = "true")
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var existingClaims = await _userManager.GetClaimsAsync(user);
            if (existingClaims.Any(c => c.Type == claimString && c.Value == value))
                return true; 

            var claim = new Claim(claimString, value);
            var result = await _userManager.AddClaimAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<List<StudentImportDto>> ImportFileStudent(IFormFile file)
        {
            var listStudent = new List<StudentImportDto>();
            using(var stream=new MemoryStream())
            {
                await file.CopyToAsync(stream);
                // Đặt ở đầu code dùng EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package=new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;    

                    for(int row=2; row <= rowCount; row++)
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
                        else if (student.StudentCode.Length !=10 )
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
        //public async Task<AddSingleStuden>AddSingleStudent(AddSingleStuden student)
        //{
        //    var user = new AppUser
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = student.StudentCode,
        //        Password = student.Password,
        //        Email = student.Email,
        //        FullName = student.FullName,
        //        Status = student.Status,
               
        //    };
        //    var result = await _userRepository.AddAsync(user);
        //    return result;
        //}

    }

}

