namespace QuizSystem_backend.DTOs.CourseClassDtos
{
    public class CourseClassSearchDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? ClassCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
