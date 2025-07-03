using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomExamsController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly IRoomExamService _roomExamServices;

        public RoomExamsController(IRoomExamService roomExamService)
        {
            _roomExamServices = roomExamService;
        }

        //// GET: api/RoomExams
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<RoomExam>>> GetRoomExams()
        //{
        //    return await _context.RoomExams.ToListAsync();
        //}

        //// GET: api/RoomExams/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<RoomExam>> GetRoomExam(Guid id)
        //{
        //    var roomExam = await _context.RoomExams.FindAsync(id);

        //    if (roomExam == null)
        //    {
        //        return NotFound();
        //    }

        //    return roomExam;
        //}

        //// PUT: api/RoomExams/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRoomExam(Guid id, RoomExam roomExam)
        //{
        //    if (id != roomExam.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(roomExam).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RoomExamExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/RoomExams
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<RoomExam>> PostRoomExam(RoomExam roomExam)
        //{
        //    _context.RoomExams.Add(roomExam);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRoomExam", new { id = roomExam.Id }, roomExam);
        //}

        //// DELETE: api/RoomExams/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRoomExam(Guid id)
        //{
        //    var roomExam = await _context.RoomExams.FindAsync(id);
        //    if (roomExam == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.RoomExams.Remove(roomExam);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        //private bool RoomExamExists(Guid id)
        //{
        //    return _context.RoomExams.Any(e => e.Id == id);
        //}


        //[HttpPost("AddExam")]
        //public async Task<IActionResult> CreateRoomExam([FromBody] RoomExamDto dto)
        //{
        //    var room = new RoomExam
        //    {
        //        Name = dto.Name,
        //        StartDate = dto.StartDate,
        //        EndTime = dto.EndTime
        //    };
        //    _dbContext.RoomExams.Add(room);
        //    await _dbContext.SaveChangesAsync();
        //    return Ok(room);
        //}

        //[HttpGet("GetAllRoomExams")]
        //public async Task<ActionResult<IEnumerable<RoomExamDto>>> GetAllRoomExams()
        //{
        //    var roomExams = await _roomExamServices.GetAllRoomExamsAsync();
        //    if (roomExams == null || !roomExams.Any())
        //    {
        //        return NotFound(new { message = "No room exams found." });
        //    }
        //    return Ok(new
        //    {
        //        code = 200,
        //        message = "Success",
        //        data = roomExams
        //    });
        //}

    }
}
