using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.DTOs.TeacherDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;
using QuizSystem_backend.services.SearcheServices;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly SearchUserService _searchUserService;
        private readonly QuizSystemDbContext _context;
        private readonly QuizSystemDbContext _dbContext;
        private readonly ITeacherService _teacherService;


        public TeachersController(QuizSystemDbContext context, QuizSystemDbContext dbContext,ITeacherService teacherService, SearchUserService searchUserService)
        {
            _context = context;
            _dbContext= dbContext;
            _teacherService= teacherService;
            _searchUserService= searchUserService;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            var teachers = await _context.Teachers.ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Cập nhật thông tin thành công",
                data = teachers
            });
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                code = 200,
                message = "Cập nhật thông tin thành công",
                data = teacher
            });
        }

        [HttpGet("{id}/classes")]

        public async Task<ActionResult> GetClassByTeacher(Guid id)
        {
            var classes = await _context.CourseClasses.Where(cc => cc.UserId == id).ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = classes
            });
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);


            if (user == null)
                return NotFound(new { message = "User not found" });

            if (id != user.Id)
            {
                return BadRequest();
            }

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
                data = user
            });
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Import-Preview")]
        public async Task<IActionResult> ImportTeachers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var listTeacher = await _teacherService.ImportFileTeacherPreview(file);

            if (listTeacher == null || !listTeacher.Any())
            {
                return BadRequest("No valid teachers found in the file.");
            }
            return Ok(listTeacher);

        }
        [HttpPost("Import-Confirm")]
        public async Task<IActionResult> ImportTeacherConfirm(List<TeacherImportDto> teachersPreview)
        {
            if (teachersPreview == null || !teachersPreview.Any())
            {
                return BadRequest("No teachers to import.");
            }
            var result = await _teacherService.ImportTeacherConfirm(teachersPreview);
            if (result.Count == 0)
            {
                return BadRequest("Failed to import teachers.");
            }
            return Ok(new
            {
                code = 200,
                message = "Success",
                data = result
            });
        }
        [HttpGet("SearchTeachers")]
        public async Task<IActionResult> SearchTeachers([FromQuery] string? keyword)
        {
            var teachers = await _searchUserService.SearchUsersAsync(Role.TEACHER, keyword);
            return Ok(teachers);
        }

        private bool TeacherExists(Guid id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
