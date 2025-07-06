using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomExamController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly IRoomExamService _roomExamService;
        private readonly IMailService _mailService;

        public RoomExamController(IRoomExamService roomExamService, QuizSystemDbContext context,IMailService mailService)
        {
            _context = context;
            _roomExamService = roomExamService;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<ActionResult> GetRoomExams()
        {
            try
            {
                // Simulate fetching room exams from a service or repository
                var roomExams = await _roomExamService.GetAllRoomExamsAsync(); // Replace with actual data fetching logic
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
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomExamById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid room exam ID." });
            }
            try
            {
                // Simulate fetching a room exam by ID from a service or repository
                var roomExam = await _roomExamService.GetRoomExamByIdAsync(id);
                if (roomExam == null)
                {
                    return NotFound(new { message = $"Room exam with ID {id} not found." });
                }
                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = roomExam
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

    }
}