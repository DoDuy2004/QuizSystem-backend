using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs.ChapterDtos;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QuizSystemDbContext _context;

        public ChaptersController(QuizSystemDbContext context,IMapper mapper)
        {
            _mapper=mapper;
            _context = context;
        }

        // GET: api/Chapters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chapter>>> GetChapters()
        {
            return await _context.Chapters.ToListAsync();
        }

        // GET: api/Chapters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chapter>> GetChapter(Guid id)
        {
            var chapter = await _context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return chapter;
        }

        // PUT: api/Chapters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChapter(Guid id, ChapterInfoDto chapterDto)
        {
            var chapter=_context.Chapters.Find(id);
            if (chapter == null) { return NotFound(); }
            
            chapter.Name=chapterDto.Name;
            chapter.Description=chapterDto.Description;
            chapter.Status=chapterDto.Status;
            await _context.SaveChangesAsync();
            return Ok(chapterDto);
        }

        // POST: api/Chapters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddChapter/{subjectId}")]
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


        // DELETE: api/Chapters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(new List<Chapter>());

            var result = await _context.Chapters
                .Where(c => c.Name.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }

        private bool ChapterExists(Guid id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }
    }
}
