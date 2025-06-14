using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratedExamsController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;

        public GeneratedExamsController(QuizSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/GeneratedExams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneratedExam>>> GetGeneratedExam()
        {
            return await _context.GeneratedExam.ToListAsync();
        }

        // GET: api/GeneratedExams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneratedExam>> GetGeneratedExam(int id)
        {
            var generatedExam = await _context.GeneratedExam.FindAsync(id);

            if (generatedExam == null)
            {
                return NotFound();
            }

            return generatedExam;
        }

        // PUT: api/GeneratedExams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneratedExam(int id, GeneratedExam generatedExam)
        {
            if (id != generatedExam.Id)
            {
                return BadRequest();
            }

            _context.Entry(generatedExam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneratedExamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GeneratedExams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeneratedExam>> PostGeneratedExam(GeneratedExam generatedExam)
        {
            _context.GeneratedExam.Add(generatedExam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneratedExam", new { id = generatedExam.Id }, generatedExam);
        }

        // DELETE: api/GeneratedExams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneratedExam(int id)
        {
            var generatedExam = await _context.GeneratedExam.FindAsync(id);
            if (generatedExam == null)
            {
                return NotFound();
            }

            _context.GeneratedExam.Remove(generatedExam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneratedExamExists(int id)
        {
            return _context.GeneratedExam.Any(e => e.Id == id);
        }
    }
}
