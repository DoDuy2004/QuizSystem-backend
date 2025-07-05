namespace QuizSystem_backend.DTOs.RoomExamDtos
{
    public class AddRoomExamResult
    {
        public bool Success { get; set; }
        public RoomExamDto? RoomExam { get; set; } 
        public string? ErrorMessages { get; set; } = "";


    }
}
