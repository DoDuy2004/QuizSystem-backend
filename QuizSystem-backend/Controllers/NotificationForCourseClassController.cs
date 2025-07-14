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

        [HttpPost("{courseClassId}")]
        public async Task<IActionResult> PostNotificationForCourseClass(Guid courseClassId, [FromBody] string content)
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
                Content = content,
                CreateAt = DateTime.Now,

            };
            _context.NotificationForCourseClass.Add(notification);

            await _context.SaveChangesAsync();

            var listStudent = await _courseClassService.GetStudentByCourseClassAsync(courseClass.Id);
            if (!listStudent.Any())
            {
                return CreatedAtAction("GetNotificationForCourseClass", new { id = notification.Id }, new { notification, note = "Không có sinh viên nào trong lớp" });
            }

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
            return CreatedAtAction("GetNotificationForCourseClass", new { id = notification.Id }, notification);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationForCourseClass(Guid id)
        {
            return Ok(await _context.NotificationForCourseClass.FindAsync(id));
        }
        [HttpPost("{notificationId}/AddMessage")]
        public async Task<IActionResult>AddMessage(Guid notificationId,string message )
        {
            
            if (message == string.Empty) return NoContent();
            

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var notification = await _context.NotificationForCourseClass.FindAsync(notificationId);
            if (notification == null) return NotFound("notification not found");

            var messe = new NotificationMessage
            {
                Id = Guid.NewGuid(),
                Content = message,
                CreateAt = DateTime.Now,
                UserId= userId,
                NotificationId= notificationId
            };

            _context.NotificationMessage.Add(messe);
            await _context.SaveChangesAsync();
            
            return Ok(messe);
        }
        [HttpGet("{notificationId}/GetMessage")]
        public async Task<IActionResult> AddMessage(Guid notificationId)
        {

            var notification = await _context.NotificationForCourseClass.FindAsync(notificationId);
            if (notification == null) return NotFound("notification not found");

            return Ok(notification.Messages);

        }



    }
}
