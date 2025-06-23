using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.services;
using System.Drawing;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        { 
            _userService = userService;
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
    }
}
