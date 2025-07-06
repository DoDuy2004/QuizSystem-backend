using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "TEACHER")]
    public class StudentsController : ControllerBase
    {
        private readonly IRoomExamRepository _roomExamRepository;
        private readonly QuizSystemDbContext _context;
        private readonly IStudentService _studentService;
        private readonly IRoomExamService _roomExamService;
        private readonly IStudentExamRepository _studentExamRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentsController(QuizSystemDbContext context, IStudentService studentService, IRoomExamService roomExamService,IStudentExamRepository studentExamRepository,IRoomExamRepository roomExamRepository,IStudentRepository studentRepository)
        {
            _roomExamRepository = roomExamRepository;
            _context = context;
            _studentService = studentService;
            _roomExamService = roomExamService;
            _studentExamRepository = studentExamRepository;
            _studentRepository=studentRepository;
        }

        // GET: api/Students
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = students
            });
        }

        [HttpGet("{id}/classes")]
        public async Task<ActionResult> GetClassByStudent(Guid id)
        {
            var classes = await _context.CourseClasses
                    .Where(cc => cc.Students.Any(s => s.StudentId == id))
                    .ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = classes
            });
        }

        // GET: api/Students/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }


            return Ok(new
            {
                code = 200,
                message = "Success",
                data = student
            });
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, [FromBody] UpdateUserDto dto)
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpPost("Import-Preview")]
        public async Task<IActionResult> ImportStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var listStudent = await _studentService.ImportFileStudentPreview(file);

            if (listStudent == null || !listStudent.Any())
            {
                return BadRequest("No valid students found in the file.");
            }
            return Ok(listStudent);

        }


        [HttpGet("search")]
        public async Task<ActionResult> SeachStudents([FromQuery] string key, [FromQuery] int limit)
        {
            if (string.IsNullOrEmpty(key) || limit <= 0)
            {
                return BadRequest(new { message = "Invalid search parameters." });
            }

            var listStudent = await _studentService.GetStudentsAsync();
            if (listStudent == null || !listStudent.Any())
            {
                return NotFound(new { message = "No students found." });
            }
            string searchKey = key.ToLowerInvariant();

            listStudent = listStudent.Where(s => s.StudentCode.ToLowerInvariant().Contains(searchKey) ||
                                                 s.FullName.ToLowerInvariant().Contains(searchKey) ||
                                                 s.Email.ToLowerInvariant().Contains(searchKey))
                                     .Take(limit).ToList();
            return Ok(new
            {
                code = 200,
                message = "Success",
                data = listStudent.Select(s => new StudentDto(s))
            });
        }
        [HttpPost("Import-Confirm")]
        public async Task<IActionResult> ImportStudentsConfirm(List<StudentImportDto> studentsPreview)
        {
            if (studentsPreview == null || !studentsPreview.Any())
            {
                return BadRequest("No students to import.");
            }
            var result = await _studentService.ImportStudentConfirm(studentsPreview);
            if (result.Count == 0)
            {
                return BadRequest("Failed to import students.");
            }
            return Ok(new
            {
                code = 200,
                message = "Success",
                data = result
            });
        }

        [HttpGet("GetRoomExams")]
        
        public async Task<ActionResult> GetRoomExam()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(role))
                return Unauthorized(new { message = "User not authenticated or role missing" });

            var userId = Guid.Parse(userIdStr);

            // Nếu là STUDENT
            if (role == "STUDENT")
            {
                var roomExams = await _context.RoomExams
                    .Include(r => r.Subject)
                    .Include(r => r.Course)
                    .Include(r => r.Exams)
                    .Where(re => _context.StudentCourseClasses
                        .Any(scc => scc.StudentId == userId && scc.CourseClassId == re.CourseClassId))
                    .ToListAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = roomExams
                });
            }

            // Nếu là TEACHER
            if (role == "TEACHER")
            {
                var now = DateTime.Now;

                var roomExams = await _context.RoomExams
                    .Include(r => r.Exams)
                    .Where(r => r.Exams.Any(e => e.UserId == userId) // Phòng thi này có exam của giáo viên đang đăng nhập
                        && r.Exams.Any(e => r.StartDate.AddMinutes(e.DurationMinutes) > now)) // Phòng thi này còn hạn
                    .Select(r => new
                    {
                        RoomExamId = r.Id,
                        RoomExamName = r.Name,
                        StartDate = r.StartDate,
                        // Lấy exam đầu tiên (theo giáo viên) để tính EndDate
                        EndDate = r.Exams
                            .Where(e => e.UserId == userId)
                            .OrderBy(e => e.Id)
                            .Select(e => r.StartDate.AddMinutes(e.DurationMinutes))
                            .FirstOrDefault(),
                        Exams = r.Exams.Where(e => e.UserId == userId).ToList()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = roomExams
                });
            }

            // Không phải 2 role trên thì từ chối truy cập
            return Forbid();
        }


        [HttpGet("{id}/roomexams")]
        public async Task<ActionResult> GetRoomExamByStudent(Guid id)
        {
            var roomExams = await _context.RoomExams
                .Where(re => _context.StudentCourseClasses
                    .Any(scc => scc.StudentId == id && scc.CourseClassId == re.CourseClassId))
                .ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = roomExams
            });
        }


        [HttpPost("submit-exam")]
        public async Task<IActionResult> SubmitExam([FromBody] SubmitStudentExamDto resultDto)
        {
            var existingSubmission = await _context.StudentRoomExams
            .FirstOrDefaultAsync(x => x.StudentId == resultDto.StudentId && x.RoomExamId == resultDto.RoomId);

            if (existingSubmission != null && existingSubmission.SubmitStatus == SubmitStatus.Submitted)
            {
                // Trả về null hoặc throw tùy bạn
                throw new Exception("Bài thi đã được nộp. Không thể nộp lại.");
            }

            if (existingSubmission == null)
            {
                _context.StudentRoomExams.Add(new StudentRoomExam
                {
                    StudentId = resultDto.StudentId,
                    RoomExamId = resultDto.RoomId,
                    SubmitStatus = SubmitStatus.Submitted,
                    SubmittedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingSubmission.SubmitStatus = SubmitStatus.Submitted;
                existingSubmission.SubmittedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            if (resultDto == null)
            {
                return BadRequest("Invalid exam submission data.");
            }
            var result=await _studentService.SubmitStudentExamAsync(resultDto);
           
            if (result == null)
            {
                return BadRequest("Failed to submit exam.");
            }
            return Ok(new
            {
                code = 200,
                message = "Success",
                data = result
            });
        }

        [HttpGet("GetStudenExam")]
        public async Task<IActionResult> GetStudentExam(Guid studentId)
        {
            var listStudentExam= await _studentExamRepository.GetListStudentExamAsync(studentId);

            return Ok(listStudentExam);
        }
        
        [HttpGet("{roomId}/is-submitted")]
        public async Task<IActionResult> CheckIfSubmitted(Guid roomId, [FromQuery] Guid studentId)
        {
            var record = await _context.StudentRoomExams
                .FirstOrDefaultAsync(x => x.RoomExamId == roomId && x.StudentId == studentId);

            if (record != null && record.SubmitStatus == SubmitStatus.Submitted)
            {
                return Ok(new { submitted = true });
            }

            return Ok(new { submitted = false });
        }
    }
}