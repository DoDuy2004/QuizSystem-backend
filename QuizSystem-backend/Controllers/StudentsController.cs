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
using QuizSystem_backend.services.SearcheServices;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "TEACHER")]
    public class StudentsController : ControllerBase
    {
        //private readonly SearchUserService _searchUserService;
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
            //_searchUserService = searchUserService;
        }

        // GET: api/Students
        [HttpGet]

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(string searchText = null)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchText) && searchText.Length >= 3)
            {
                query = query.Where(s =>
                    s.FullName.Contains(searchText) ||
                    s.Email.Contains(searchText) ||
                    s.StudentCode.Contains(searchText) ||
                    s.Facutly.Contains(searchText));
            }

            var students = await query.ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Lấy danh sách sinh viên thành công",
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




        //[HttpGet("{id}/roomexams")]
        //public async Task<ActionResult> GetRoomExamByStudent(Guid id)
        //{
        //    var roomExams = await _context.RoomExams
        //        .Include(re => re.Subject)
        //        .Include(re => re.Course)
        //        .Include(re => re.Exams)
        //        .Where(re => _context.StudentCourseClasses
        //            .Any(scc => scc.StudentId == id && scc.CourseClassId == re.CourseClassId))
        //        .ToListAsync();

        //    return Ok(new
        //    {
        //        code = 200,
        //        message = "Success",
        //        data = roomExams
        //    });
        //}

        private int CalculateTimeRemaining(DateTime startDate, int durationMinutes)
        {
            var now = DateTime.UtcNow;
            var endDate = startDate.AddMinutes(durationMinutes);
            var timeSpan = endDate - now;

            if (now >= startDate && timeSpan.TotalSeconds > 0)
            {
                return (int)timeSpan.TotalSeconds;
            }
            else if (now < startDate)
            {
                return (int)(startDate - now).TotalSeconds;
            }
            // Nếu đã hết giờ, trả về 0
            return 0;
        }

        [HttpGet("{id}/roomexams")]
        public async Task<ActionResult> GetRoomExamByStudent(Guid id, string searchText = null)
        {
            var query = _context.RoomExams
                .Include(re => re.Subject)
                .Include(re => re.Course)
                .Include(re => re.Exams)
                .Where(re => _context.StudentCourseClasses
                    .Any(scc => scc.StudentId == id && scc.CourseClassId == re.CourseClassId));

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(re =>
                    re.Name.Contains(searchText) ||
                    (re.Subject != null && re.Subject.Name.Contains(searchText)) ||
                    (re.Course != null && re.Course.Name.Contains(searchText)));
            }

            var roomExams = await query.ToListAsync();

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

            var studentExists = await _context.Students.AnyAsync(s => s.Id == resultDto.StudentId);
            if (!studentExists)
            {
                return BadRequest("Sinh viên không tồn tại.");
            }

            var existingSubmission = await _context.StudentRoomExams
            .FirstOrDefaultAsync(x => x.StudentId == resultDto.StudentId && x.RoomExamId == resultDto.RoomId);

            if (existingSubmission != null && existingSubmission.SubmitStatus == SubmitStatus.Submitted)
            {
                throw new Exception("Bài thi đã được nộp. Không thể nộp lại.");
            }

            if (existingSubmission == null)
            {
                _context.StudentRoomExams.Add(new StudentRoomExam
                {
                    StudentId = resultDto.StudentId,
                    RoomExamId = resultDto.RoomId,
                    SubmitStatus = SubmitStatus.Submitted,
                    SubmittedAt = DateTime.Now
                });
            }
            else
            {
                existingSubmission.SubmitStatus = SubmitStatus.Submitted;
                existingSubmission.SubmittedAt = DateTime.Now;
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

        //[HttpGet("SearchStudents")]
        //public async Task<IActionResult> SearchStudents([FromQuery] string? keyword)
        //{
        //    var students = await _searchUserService.SearchUsersAsync(Role.STUDENT, keyword);
        //    return Ok(students);
        //}


        [HttpGet("{studentId}/studentExamDetail/{id}")]
        public IActionResult GetStudentExamDetail(Guid id, Guid studentId)
        {
            try
            {
                var studentExam = _context.StudentExams
                    .Include(se => se.Exam)
                        .ThenInclude(e => e.Subject)
                    .Include(se => se.StudentExamDetails)
                        .ThenInclude(sed => sed.Question)
                            .ThenInclude(q => q.Answers)
                    .Include(se => se.Student)
                    .Include(se => se.Room)
                        .ThenInclude(re => re.StudentRoomExams)
                    .FirstOrDefault(se => se.Id == id);

                if (studentExam == null || studentExam.StudentId != studentId)
                {
                    return NotFound("Bài thi không tồn tại hoặc không thuộc về sinh viên.");
                }

                var studentRoomExam = studentExam.Room?.StudentRoomExams
                    ?.FirstOrDefault(sre => sre.StudentId == studentId);

                DateTime? submittedDateTime = studentRoomExam?.SubmittedAt;
                string submittedAt = submittedDateTime?.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime? startDate = studentExam.Room?.StartDate;
                double durationTaken = (submittedDateTime.HasValue && startDate.HasValue)
                    ? (submittedDateTime.Value - startDate.Value).TotalMinutes
                    : 0;

                var examSubjectName = studentExam.Exam?.Subject?.Name ?? "Không rõ";

                var response = new
                {
                    exam = new
                    {
                        id = studentExam.Exam?.Id,
                        name = studentExam.Exam?.Name,
                        subject = new { name = examSubjectName },
                        durationMinutes = studentExam.Exam.DurationMinutes
                    },
                    student = new
                    {
                        id = studentExam.Student?.Id,
                        fullName = studentExam.Student?.FullName ?? "Không rõ",
                        email = studentExam.Student?.Email ?? "Không rõ"
                    },
                    submittedAt = submittedAt,
                    durationTaken = durationTaken,
                    grade = studentExam.Grade, // Thêm điểm số từ CSDL
                    questions = studentExam.StudentExamDetails
                        .Where(sed => sed.Question != null)
                        .Select(sed => sed.Question)
                        .DistinctBy(q => q.Id)
                        .Select(q => new
                        {
                            id = q.Id,
                            content = q.Content,
                            type = q.Type,
                            answers = q.Answers.Select(a => new
                            {
                                id = a.Id,
                                content = a.Content,
                                isCorrect = a.IsCorrect
                            }).ToList(),
                            correctAnswerIds = q.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToList()
                        }).ToList(),
                    studentAnswers = studentExam.StudentExamDetails
                        .GroupBy(d => d.QuestionId)
                        .ToDictionary(
                            g => g.Key.ToString(),
                            g => g.Select(d => d.AnswerId.ToString()).ToList()
                        )
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

    }
}