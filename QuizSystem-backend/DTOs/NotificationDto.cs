using QuizSystem_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizSystem_backend.DTOs
{
    public class NotificationDto
    {
        
        Guid Id { get; set; } = Guid.NewGuid();
        string Content { get; set; } = string.Empty;
        Guid CourseClassId { get; set; }
        Guid TeacherId { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
       
    }
}
