using QuizSystem_backend.Enums;

namespace QuizSystem_backend.DTOs.StudentDtos
{
    public class StudentInRoom
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;

        public SubmitStatus SubmitStatus = SubmitStatus.NotSubmitted;
   
    }
}
