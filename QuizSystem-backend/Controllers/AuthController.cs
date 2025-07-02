using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.AuthDto;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, TokenService tokenService,IMapper mapper,UserManager<AppUser>userManager,SignInManager<AppUser>signInManager,IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByUsernameAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Tài khoản hoặc mật khẩu không đúng"
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            // Tạo claims cho token
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddHours(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user = new UserDto(user)
                }
            });
        }

        //[HttpPost("token")]
        //[Authorize]
        //public async Task<ActionResult> AuthenticateByToken()
        //{
        //    // Lấy userId từ token
        //    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(new
        //        {
        //            code = 401,
        //            message = "Token không hợp lệ hoặc đã hết hạn"
        //        });
        //    }

        //    // Lấy thông tin người dùng từ CSDL
        //    var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
        //    if (user == null)
        //    {
        //        return Unauthorized(new
        //        {
        //            code = 401,
        //            message = "Người dùng không tồn tại"
        //        });
        //    }

        //    var token = _tokenService.CreateToken(user);

        //    return Ok(new
        //    {
        //        code = 200,
        //        message = "Authenticated by token",
        //        data = new
        //        {
        //            token,
        //            user = new UserDto(user)
        //        }
        //    });
        //}


        //[HttpPost("logout")]
        //[Authorize]
        //public ActionResult Logout()
        //{
        //    return Ok(new
        //    {
        //        code = 200,
        //        message = "Đăng xuất thành công"
        //    });
        //}
        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new
            {
                code = 200,
                message = "Đăng xuất thành công"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("teacher/{teacherId}/{Permission}")]
        public async Task<IActionResult> GrantPermission(Guid teacherId,string Permission)
        {
            var user = await _userManager.FindByIdAsync(teacherId.ToString());

            if (user == null) return NotFound();

            var existingClaims = await _userManager.GetClaimsAsync(user);

            if (existingClaims.Any(c => c.Type == Permission && c.Value == "true"))
                return BadRequest(new { message = "Quyền đã được thêm trước đó" });

            await _userManager.AddClaimAsync(user, new Claim(Permission, "true"));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DELETE/teacher/{teacherId}/{Permission}")]
        public async Task<IActionResult>RemovePermission(Guid teacherId, string Permission)
        {
            var user = await _userManager.FindByIdAsync(teacherId.ToString());

            if (user == null) return NotFound("User not found!");

            var existingClaims = await _userManager.GetClaimsAsync(user);

            var claimRemove=existingClaims.FirstOrDefault(c=>c.Type == Permission && c.Value == "true");    

            if(claimRemove == null)
                return BadRequest(new { message = "Quyền không tồn tại hoặc đã bị xóa trước đó" });

            await _userManager.RemoveClaimAsync(user,claimRemove);
            
            return Ok();
        }
    }
}
