namespace QuizSystem_backend.Models
{
    public class NotificationForCourseClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public Guid CourseClassId { get; set; }
        
        //
        public CourseClass CourseClass { get; set; } = null!;
        public ICollection<NotificationMessage> Messages { get; set; }=new List<NotificationMessage>();
    }
}
