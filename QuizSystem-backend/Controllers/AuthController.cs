using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;

        public AuthController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByUsernameAsync(dto.Username);

            if (user == null) 
            {
                return Unauthorized("User not found");
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return Unauthorized("Wrong password");

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
    }
}
