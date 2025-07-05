using QuizSystem_backend.DTOs.ExamDtos;

namespace QuizSystem_backend.DTOs.RoomExamDtos
{
    public class AddRoomExamResult
    {
        public bool Success { get; set; }
        public AddRoomExamDto? RoomExam { get; set; } 
        public string? ErrorMessages { get; set; } = "";


    }
}
