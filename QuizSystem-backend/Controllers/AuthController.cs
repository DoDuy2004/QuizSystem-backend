using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;
using QuizSystem_backend.services.MailServices;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IEmailSender _emailService;
        private readonly QuizSystemDbContext _dbContext;

        public AuthController(IUserService userService, TokenService tokenService, IEmailSender emailService, QuizSystemDbContext dbContext)
        {
            _userService = userService;
            _tokenService = tokenService;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByUsernameAsync(dto.Username);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            if (user.Status == Status.DELETED)
            {
                return Unauthorized("Tài khoản của bạn đã bị khóa.");
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Sai mật khẩu");

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = new
                {
                    token,
                    user = new UserDto(user)
                }
            });
        }

        [HttpPost("token")]
        [Authorize]
        public async Task<ActionResult> AuthenticateByToken()
        {
            // Lấy userId từ token
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Token không hợp lệ hoặc đã hết hạn"
                });
            }

            // Lấy thông tin người dùng từ CSDL
            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Người dùng không tồn tại"
                });
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                code = 200,
                message = "Authenticated by token",
                data = new
                {
                    token,
                    user = new UserDto(user)
                }
            });
        }


        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            return Ok(new
            {
                code = 200,
                message = "Đăng xuất thành công"
            });
        }

        public class ForgotPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
        }

        public class ValidateOtpRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Otp { get; set; } = string.Empty;
        }

        public class ResetPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("forgot-password/request-pin")]
        //[Authorize]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Người dùng không tồn tại"
                });
            }

            var random = new Random();
            string otp = "";
            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            user.Otp = otp;
            user.OtpExpireTime = DateTime.UtcNow.AddMinutes(5);
            await _dbContext.SaveChangesAsync();
            var mailContent = new MailContent();
            mailContent.To = request.Email;
            mailContent.Subject = "Đặt lại mật khẩu cho tài khoản EduQuiz";
            mailContent.Body = $@"
                        <p>Xin chào {user.FullName},</p>
                        <p>Bạn vừa yêu cầu đặt lại mật khẩu.</p>
                        <p>OTP của bạn là: <b style='color: #e74c3c; font-size: 20px'>{otp}</b></p>
                        <p>Otp có hiệu lực trong 5 phút</p>
                        <p>Trân trọng,<br/>EduQuiz Team</p>
                    ";

            Task.Run(() => _emailService.SendEmailAsync(mailContent.To, mailContent.Subject, mailContent.Body));

            return Ok(new
            {
                code = 200,
                message = "Emai sended"
            });
        }

        [HttpPost("ValidatePin")]
        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpRequest request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Email);
            if (user == null)
                return BadRequest("User Not Found");

            if (user.Otp == request.Otp && user.OtpExpireTime > DateTime.UtcNow)
            {
                user.Otp = null!;
                
                await _dbContext.SaveChangesAsync();

                return Ok(new { code = 200, message = "OTP hợp lệ!" });
            }

            return BadRequest(new { code = 400, message = "OTP không hợp lệ hoặc đã hết hạn!" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {

            var user = await _userService.GetUserByUsernameAsync(request.Email);
            if (user == null) return BadRequest("User Not Found");
            if (user.OtpExpireTime > DateTime.Now) return BadRequest("Time Out");

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, request.Password);

            user.OtpExpireTime = DateTime.MinValue;

            await _dbContext.SaveChangesAsync();
            return Ok("Đổi mật khẩu thành công!");
        }

    }
}
