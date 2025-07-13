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
    public class NotificationForCourseClassController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;

        public NotificationForCourseClassController(QuizSystemDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostNotificationForCourseClass(Models.NotificationForCourseClass notificationForCourseClass)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.NotificationForCourseClass.Add(notificationForCourseClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotificationForCourseClass", new { id = notificationForCourseClass.Id }, notificationForCourseClass);
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationForCourseClass(Guid id)
        {
            var notificationForCourseClass = await _context.NotificationForCourseClass.FindAsync(id);
            if (notificationForCourseClass == null)
            {
                return NotFound();
            }

            _context.NotificationForCourseClass.Remove(notificationForCourseClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
