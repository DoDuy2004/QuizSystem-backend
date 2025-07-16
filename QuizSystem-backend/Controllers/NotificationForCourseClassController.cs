using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;
using QuizSystem_backend.services.MailServices;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationForCourseClassController : ControllerBase
    {
        private readonly QuizSystemDbContext _context;
        private readonly IEmailSender _mailService;
        private readonly ICourseClassService _courseClassService;

        public NotificationForCourseClassController(QuizSystemDbContext context, IEmailSender mailService, ICourseClassService courseClassService)
        {
            _context = context;
            _mailService = mailService;
            _courseClassService = courseClassService;
        }

        public class NotificationRequest
        {
            public string Content { get; set; }
        }

        [HttpPost("{courseClassId}")]
        public async Task<IActionResult> PostNotificationForCourseClass(Guid courseClassId, [FromBody] NotificationRequest req)
        {
            var courseClass = await _context.CourseClasses.FindAsync(courseClassId);

            if (courseClass == null)
            {
                return NotFound("CourseClass not found");
            }

            var notification = new NotificationForCourseClass
            {
                Id = Guid.NewGuid(),
                CourseClassId = courseClassId,
                Content = req.Content,
                CreateAt = DateTime.Now,
            };

            _context.NotificationForCourseClass.Add(notification);
            await _context.SaveChangesAsync();

            var listStudent = await _courseClassService.GetStudentByCourseClassAsync(courseClass.Id);

            var subject = $"Thông báo mới từ lớp {courseClass.Name}";

            foreach (var st in listStudent)
            {
                var htmlMessage = $@"Xin chào {st.FullName},
                    Giảng viên vừa đăng thông báo mới cho lớp {courseClass.Name}
                    Vui lòng đăng nhập vào hệ thống để kiểm tra thông tin kỳ thi và hoàn thành bài thi đúng thời gian quy định.

                    Chúc bạn làm bài thật tốt!
                    Trân trọng,
                    Đội ngũ EduQuiz";

                Task.Run(() => _mailService.SendEmailAsync(st.Email, subject, htmlMessage));
            }

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = notification
            });
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

        [HttpDelete("{notiId}/message/{messId}")]
        public async Task<IActionResult> DeleteMessage(Guid notiId, Guid messId)
        {
            var message = await _context.NotificationMessage.FindAsync(messId);
            if (message == null)
            {
                return NotFound();
            }

            _context.NotificationMessage.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{classId}/notification")]
        public async Task<IActionResult> GetNotificationForCourseClass(Guid classId)
        {
            var noties = await _context.NotificationForCourseClass.Where(n => n.CourseClassId == classId).ToListAsync();

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = noties
            });
        }

        public class MessageRequest
        {
            public string Message { get; set; }
        }

        [HttpPost("{notificationId}/AddMessage")]
        public async Task<IActionResult>AddMessage(Guid notificationId, [FromBody] MessageRequest req )
        {
            
            if (req.Message == string.Empty) return NoContent();
            

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var notification = await _context.NotificationForCourseClass.FindAsync(notificationId);
            var user = await _context.Users.FindAsync(userId);
            if (notification == null) return NotFound("notification not found");

            var messe = new NotificationMessage
            {
                Id = Guid.NewGuid(),
                Content = req.Message,
                CreateAt = DateTime.Now,
                UserId = userId,
                User = user,
                NotificationId = notificationId
            };

            _context.NotificationMessage.Add(messe);
            await _context.SaveChangesAsync();
           
            return Ok(new
            {
                code = 200,
                message = "Success",
                data = messe
            });
        }
        [HttpGet("{notificationId}/GetMessage")]
        public async Task<IActionResult> GetMessage(Guid notificationId)
        {

            var notification = await _context.NotificationForCourseClass
                .Include(n => n.Messages)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification == null) return NotFound("notification not found");

            return Ok(new
            {
                code = 200,
                message = "Success",
                data = notification.Messages
            });
        }
    }
}
