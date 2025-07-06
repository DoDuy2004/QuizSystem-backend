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
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly QuizSystemDbContext _dbContext;

        public TeachersController(QuizSystemDbContext context, QuizSystemDbContext dbContext)
        {
            _context = context;
            _dbContext= dbContext;
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

        [HttpGet("GetRoomExam")]
        public async Task<IActionResult> GetRoomExam(Guid roomExamId,Guid teacherId)
        {
            var now = DateTime.Now;

            var roomExam = await _context.RoomExams
                .Include(r => r.Exams)
                .Where(r => r.Id == roomExamId) // điều kiện RoomExamId
                .Select(r => new
                {
                    RoomExam = r,
                    // Exam đầu tiên của giáo viên này
                    Exam = r.Exams.Where(e => e.UserId == teacherId).OrderBy(e => e.Id).FirstOrDefault()
                })
                .Where(x => x.Exam != null) // phải có Exam của teacher
                .Where(x => x.RoomExam.StartDate.AddMinutes(x.Exam.DurationMinutes) > now) // chưa hết hạn
                .FirstOrDefaultAsync();

            if (roomExam == null)
                return NotFound();

            var result = new
            {
                RoomExamId = roomExam.RoomExam.Id,
                RoomExamName = roomExam.RoomExam.Name,
                StartDate = roomExam.RoomExam.StartDate,
                EndDate = roomExam.RoomExam.StartDate.AddMinutes(roomExam.Exam.DurationMinutes),
                ExamId = roomExam.Exam.Id,
                ExamName = roomExam.Exam.Name,
                DurationMinutes = roomExam.Exam.DurationMinutes
            };

            return Ok(result);
        }

        private bool TeacherExists(Guid id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
