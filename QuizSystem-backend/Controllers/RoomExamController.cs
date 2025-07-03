using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizSystem_backend.DTOs;
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

        public RoomExamController(IRoomExamService roomExamService, QuizSystemDbContext context)
        {
            _context = context;
            _roomExamService = roomExamService;
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
        [HttpPost("CreateRoom")]
        public async Task<ActionResult> CreateRoomExam([FromBody] RoomExamDto roomExamDto)
        {

            try
            {
                await _roomExamService.AddRoomExamAsync(roomExamDto);
                return CreatedAtAction(nameof(GetRoomExamById), new { id = Guid.NewGuid() }, new { message = "Room exam created successfully." });
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
        [HttpPost("AddStudentToRoomExam")]
        public async Task<ActionResult> AddStudentToRoomExam(IFormFile file, Guid roomExamId)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required." });
            }
            try
            {
                var list = await _roomExamService.ImportStudenInRoomExam(file, roomExamId);

                return Ok(new { message = "Student added to room exam successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }


        }
    }
}