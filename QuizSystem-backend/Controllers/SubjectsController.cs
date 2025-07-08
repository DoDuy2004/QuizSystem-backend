using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.ChapterDtos;
using QuizSystem_backend.DTOs.SubjectDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "ADMIN, TEACHER")]
    public class SubjectsController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly IMapper _mapper;

        public SubjectsController(QuizSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetSubjects(string searchText = null)
        {
            var query = _context.Subjects.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(s =>
                    s.Name.Contains(searchText));
            }

            var subjects = await query.ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = subjects
            });
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(Guid id)
        {
            var subject = await _context.Subjects
                                .Include(s => s.Chapters)
                                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = subject
            }); ;
        }

        [HttpGet("{id}/chapters")]
        public async Task<ActionResult> GetChapters(Guid id)
        {
            var chapters = await _context.Chapters.Where(c => c.SubjectId == id).ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = chapters
            }); 
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSubject(Guid id, [FromBody] SubjectInfoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingSubject = await _context.Subjects
                                              .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSubject == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Subject not found"
                });
            }

            // Cập nhật thông tin Subject
            existingSubject.Name = dto.Name;
            existingSubject.SubjectCode = dto.SubjectCode;
            existingSubject.Major = dto.Major;
            existingSubject.Description = dto.Description;
            existingSubject.Status = dto.Status;


            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Subject updated successfully",
                data = existingSubject
            });
        }

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostSubject([FromBody] CreateSubjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map DTO sang entity
            var subject = _mapper.Map<Subject>(dto);

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = subject
            });
        }


        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("SearchByKeyword")]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(new List<Subject>());

            var result = await _context.Subjects
                .Where(c => c.Name.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }
        
        [HttpPost("{subjectId}/add-chapter")]
        public async Task<ActionResult<Chapter>> PostChapter(Guid subjectId, [FromBody] ChapterInfoDto chapterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null)
                return NotFound("Subject Not Found");

            var chapter = _mapper.Map<Chapter>(chapterDto);
            chapter.SubjectId = subjectId;
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return Ok(chapter);
        }


        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
