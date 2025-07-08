using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.UserDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;
using System.Drawing;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly QuizSystemDbContext _context;
        public UserController(IUserService userService, QuizSystemDbContext context) 
        { 
            _userService = userService;
            _context = context;
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult> Current([FromHeader] Guid userId)
        {
            if (Guid.Empty == userId)
            {
                return BadRequest(new { message = "UserId is required" });
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var userDto = new UserDto(user);

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = userDto
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromHeader] Guid userId, [FromBody] ChangePasswordDto dto)
        {
            if (userId == Guid.Empty)
                return BadRequest("UserId is required");

            var success = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            if (!success)
                return BadRequest("Mật khẩu cũ sai ");

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromHeader] Guid userId, [FromBody] UpdateUserDto dto)
        {
            if (userId == Guid.Empty)
                return BadRequest(new { message = "UserId is required" });

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            user.FullName = dto.FullName;
            user.Gender = dto.Gender;
            user.DateOfBirth = dto.DateOfBirth;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Cập nhật thông tin thành công",
                data=user
            });
        }

        [HttpPost("AddSingle")]
        public async Task<IActionResult> AddSingle(AddUserDtos user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            var result=await _userService.AddUser(user);
            if(!result.Succeed)
                return BadRequest(result.Message);
            await _userService.AddUser(user);
            return Ok(user);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Status = Status.DELETED;
            _context.Users.Update(user); // Không bắt buộc nhưng tốt khi bạn set thủ công
            await _context.SaveChangesAsync();

            return Ok(new {message = "Delete successfully", Status = user.Status});
        }

    }
}
