using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Hubs;
using QuizSystem_backend.services.Notificationervices;
using QuizSystem_backend.services.NotificationServices;
using System.Security.Claims;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notifSvc;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<QuizHub> _hubContext;

        public NotificationsController(IHubContext<QuizHub>hubContext,INotificationService notifSvc, INotificationService notificationService)
        {
            _notifSvc = notifSvc;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int max = 10)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var list = await _notifSvc.GetNotificationAsync(userId, max);
            return Ok(list);
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var count = await _notifSvc.GetUnreadCountAsync(userId);
            return Ok(new { unreadCount = count });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationDto dto, Guid userId)
        {
            await _notifSvc.AddNotificationAsync(userId, dto.Title, dto.Message);
            return NoContent();
        }

        [HttpPost("/read")]
        public async Task<IActionResult> MarkRead(List<NotificationDto> notificationDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notifSvc.MarkAsReadAsync(userId,notificationDto);
            return NoContent();
        }
    }
}
