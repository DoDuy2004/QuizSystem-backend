using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;
using QuizSystem_backend.services.MailServices;

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

        [HttpPost("ForgotPassword")]
        //[Authorize]
        public async Task<ActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userService.GetUserByUsernameAsync(email);
            if (user == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Người dùng không tồn tại"
                });
            }


            var newToken = _tokenService.CreateToken(user);
            user.ResetPasswordToken = newToken;
            user.ResetPasswordTokenExpire = DateTime.Now.AddMinutes(5);
            await _dbContext.SaveChangesAsync();

            string resetLink = $"https://localhost:7225/reset-password?token={newToken}&email={Uri.EscapeDataString(user.Email)}";

            var mailContent = new MailContent();
            mailContent.To = email;
            mailContent.Subject = "Đặt lại mật khẩu cho tài khoản EduQuiz";
            mailContent.Body = $@"
                                <p>Xin chào {user.FullName},</p>
                                <p>Bạn vừa yêu cầu đặt lại mật khẩu.</p>
                                <p>Để đặt lại mật khẩu, bấm vào đây: <a href=""{resetLink}"">{resetLink}</a></p>
                                <p>Nếu bạn không yêu cầu, vui lòng bỏ qua email này.</p>
                                <p>Trân trọng,<br/>EduQuiz Team</p>
                                ";

            Task.Run(() => _emailService.SendEmailAsync(mailContent.To, mailContent.Subject, mailContent.Body));

            return Ok(new
            {
                code = 200,
                message = "Đã gửi email hướng dẫn đặt lại mật khẩu"
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return BadRequest("Email không hợp lệ.");

            if (user.ResetPasswordToken != model.Token || user.ResetPasswordTokenExpire < DateTime.UtcNow)

                return BadRequest("Token không hợp lệ hoặc đã hết hạn.");

            var hasher = new PasswordHasher<User>();

            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

            user.ResetPasswordToken = null!;
            user.ResetPasswordTokenExpire = null;
            await _dbContext.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công!");
        }

    }
}
