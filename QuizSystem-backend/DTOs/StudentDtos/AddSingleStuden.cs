using System.Security.Cryptography.X509Certificates;

namespace QuizSystem_backend.DTOs.StudentDtos
{
    public class AddSingleStuden
    {
        public string StudentCode { get; set; } = null!;
        public string password { get; set; } = null!;

    }
}
