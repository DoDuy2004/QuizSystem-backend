using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;
using System.Security.Claims;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomExamController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly IRoomExamService _roomExamService;
        private readonly IEmailSender _emailSender;

        public RoomExamController(IRoomExamService roomExamService, QuizSystemDbContext context,IEmailSender emailSender)
        {
            _context = context;
            _roomExamService = roomExamService;
            _emailSender = emailSender;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetRoomExams()
        //{
        //    //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    //var role = User.FindFirst(ClaimTypes.Role)?.Value;
        //    try
        //    {
        //        // Simulate fetching room exams from a service or repository
        //        var roomExams = await _roomExamService.GetAllRoomExamsAsync(); // Replace with actual data fetching logic
        //        return Ok(new
        //        {
        //            code = 200,
        //            message = "Success",
        //            data = roomExams
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult> GetRoomExams(string searchText = null)
        {
            try
            {
                var roomExams = await _roomExamService.GetAllRoomExamsAsync();

                if (!string.IsNullOrEmpty(searchText))
                {
                    roomExams = roomExams.Where(re =>
                        re.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (re.Subject?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (re.Course?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = roomExams
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private int CalculateTimeRemaining(DateTime startDate, int durationMinutes)
        {
            var now = DateTime.Now;
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
            return 0;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomExamById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid room exam ID." });
            }
            try
            {
                var roomExam = await _roomExamService.GetRoomExamByIdAsync(id);
                if (roomExam == null)
                {
                    return NotFound(new { message = $"Room exam with ID {id} not found." });
                }

                // Tính timeRemaining
                var timeRemaining = CalculateTimeRemaining(roomExam.StartDate, roomExam.Exams[0]?.DurationMinutes ?? 0);

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = new
                    {
                        roomExam.Id,
                        roomExam.Name,
                        Subject = roomExam.Subject,
                        Course = roomExam.Course,
                        StartDate = roomExam.StartDate,
                        DurationMinutes = roomExam.Exams[0]?.DurationMinutes ?? 0,
                        TimeRemaining = timeRemaining,
                        Exams = roomExam.Exams
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoomExam(Guid id, [FromBody] string roomExam)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(roomExam))
            {
                return BadRequest(new { message = "Invalid room exam ID or data." });
            }
            try
            {
                // Simulate updating a room exam in a service or repository
                // Replace with actual update logic
                return Ok(new { message = "Room exam updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoomExam(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid room exam ID." });
            }
            try
            {
                // Simulate deleting a room exam in a service or repository
                // Replace with actual deletion logic
                return Ok(new { message = "Room exam deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddRoomExam([FromBody] AddRoomExamDto roomExamDto)
        {
            if (roomExamDto == null)
            {
                return BadRequest(new { message = "Room exam data is required." });
            }

            try
            {
                var result = await _roomExamService.AddRoomExamAsync(roomExamDto);
                if (!result.Success)
                {
                    return BadRequest(new { message = result.ErrorMessages });
                }
                return CreatedAtAction(nameof(GetRoomExamById), new { id = result.RoomExam.Id }, result.RoomExam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpGet("GetRoomExams")]
        public async Task<ActionResult> GetRoomExamResult(string searchText = null)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(role))
                return Unauthorized(new { message = "User not authenticated or role missing" });

            var userId = Guid.Parse(userIdStr);
            searchText = searchText?.Trim()?.ToLower();

            if (role == "STUDENT")
            {
                var result = await _context.StudentExams
                    .Where(se => se.StudentId == userId)
                    .Select(se => new
                    {
                        se.Id,
                        se.Grade,
                        se.Note,
                        se.Status,
                        se.SubmitStatus,
                        Exam = new
                        {
                            se.Exam.Id,
                            se.Exam.Name,
                            se.Exam.DurationMinutes
                        },
                        RoomExam = new
                        {
                            se.Room.Id,
                            se.Room.Name,
                            se.Room.StartDate,
                            se.Room.EndDate,
                            subject = se.Room.Subject.Name
                        }
                    })
                    .ToListAsync();

                // 👉 Lọc kết quả theo searchText
                if (!string.IsNullOrEmpty(searchText))
                {
                    result = result.Where(x =>
                        x.Exam.Name.ToLower().Contains(searchText) ||
                        x.RoomExam.Name.ToLower().Contains(searchText) ||
                        x.RoomExam.subject.ToLower().Contains(searchText))
                        .ToList();
                }

                return Ok(new { code = 200, message = "Success", data = result });
            }

            if (role == "TEACHER")
            {
                var now = DateTime.Now;

                var endedRoomExams = await _context.RoomExams
                    .Include(r => r.Exams)
                    .Where(r =>
                        r.Exams.Any(e => e.UserId == userId) &&
                        r.Exams.Any(e => r.StartDate.AddMinutes(e.DurationMinutes) <= now)
                    )
                    .Select(r => new
                    {
                        RoomExamId = r.Id,
                        RoomExamName = r.Name,
                        Subject = r.Subject.Name,
                        StartDate = r.StartDate,
                        EndDate = r.Exams
                            .Where(e => e.UserId == userId)
                            .OrderBy(e => e.Id)
                            .Select(e => r.StartDate.AddMinutes(e.DurationMinutes))
                            .FirstOrDefault(),
                        Exams = r.Exams
                            .Where(e => e.UserId == userId)
                            .Select(e => new
                            {
                                e.Id,
                                e.Name
                            }).ToList(),
                        TotalStudentExams = _context.StudentExams.Count(se => se.RoomId == r.Id)
                    })
                    .ToListAsync();

                // 👉 Lọc theo searchText
                if (!string.IsNullOrEmpty(searchText))
                {
                    endedRoomExams = endedRoomExams.Where(x =>
                        x.RoomExamName.ToLower().Contains(searchText) ||
                        x.Subject.ToLower().Contains(searchText))
                        .ToList();
                }

                return Ok(new { code = 200, message = "Success", data = endedRoomExams });
            }

            return Forbid();
        }


        [HttpGet("{roomExamId}/student-exams")]
        public async Task<IActionResult> GetStudentExamsByRoom(Guid roomExamId)
        {
            var result = await _context.StudentExams
                .Select(se => new
                {
                    se.Id,
                    se.Grade,
                    se.Note,
                    se.Status,
                    se.SubmitStatus,

                    // ✅ Thông tin sinh viên
                    Student = new
                    {
                        se.Student.Id,
                        se.Student.FullName,
                        se.Student.Email,
                        se.Student.PhoneNumber,
                        se.Student.StudentCode
                    },

                    // ✅ Thông tin bài thi
                    Exam = new
                    {
                        se.Exam.Id,
                        se.Exam.Name,
                        se.Exam.DurationMinutes
                    },

                    // ✅ Thông tin phòng thi
                    RoomExam = new
                    {
                        se.Room.Id,
                        se.Room.Name,
                        se.Room.StartDate,
                        se.Room.EndDate,
                        SubjectName = se.Room.Subject.Name
                    }
                })
                .Where(se => se.RoomExam.Id == roomExamId)
                .ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = result
            });

        }

    }
}