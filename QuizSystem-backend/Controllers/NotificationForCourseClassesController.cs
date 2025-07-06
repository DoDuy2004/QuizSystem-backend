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
    public class NotificationForCourseClassesController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;

        public NotificationForCourseClassesController(QuizSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/NotificationForCourseClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationForCourseClass>>> GetNotificationForCourseClasses()
        {
            return await _context.NotificationForCourseClasses.ToListAsync();
        }

        // GET: api/NotificationForCourseClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationForCourseClass>> GetNotificationForCourseClass(Guid id)
        {
            var notificationForCourseClass = await _context.NotificationForCourseClasses.FindAsync(id);

            if (notificationForCourseClass == null)
            {
                return NotFound();
            }

            return notificationForCourseClass;
        }

        // PUT: api/NotificationForCourseClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationForCourseClass(Guid id, NotificationForCourseClass notificationForCourseClass)
        {
            if (id != notificationForCourseClass.Id)
            {
                return BadRequest();
            }

            _context.Entry(notificationForCourseClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationForCourseClassExists(id))
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

        // POST: api/NotificationForCourseClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotificationForCourseClass>> PostNotificationForCourseClass(NotificationForCourseClass notificationForCourseClass)
        {
            _context.NotificationForCourseClasses.Add(notificationForCourseClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotificationForCourseClass", new { id = notificationForCourseClass.Id }, notificationForCourseClass);
        }

        // DELETE: api/NotificationForCourseClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationForCourseClass(Guid id)
        {
            var notificationForCourseClass = await _context.NotificationForCourseClasses.FindAsync(id);
            if (notificationForCourseClass == null)
            {
                return NotFound();
            }

            _context.NotificationForCourseClasses.Remove(notificationForCourseClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationForCourseClassExists(Guid id)
        {
            return _context.NotificationForCourseClasses.Any(e => e.Id == id);
        }
    }
}
