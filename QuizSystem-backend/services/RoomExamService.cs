using AutoMapper;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;

namespace QuizSystem_backend.services
{
    public class RoomExamService
    {
        private readonly IRoomExamRepository _roomExamRepository;
        private readonly IMapper _mapper;

        public RoomExamService(IRoomExamRepository roomExamRepository,IMapper mapper)
        {
            _roomExamRepository = roomExamRepository;
            _mapper = mapper;
        }
        public async Task<RoomExamDto> AddRoomExamAsync(RoomExamDto roomExamDto)
        {
            if (roomExamDto == null) return null!;

            var roomExam = _mapper.Map<RoomExam>(roomExamDto);

            var addedRoomExam = await _roomExamRepository.AddAsync(roomExam);

            return _mapper.Map<RoomExamDto>(addedRoomExam);
        }

        public async Task<IEnumerable<RoomExamDto>> GetAllRoomExamsAsync()
        {
            var roomExams = await _roomExamRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomExamDto>>(roomExams);
        }

        public async Task<RoomExamDto> GetRoomExamByIdAsync(Guid id)
        {
            var roomExam = await _roomExamRepository.GetByIdAsync(id);
            if (roomExam == null) return null!;

            return _mapper.Map<RoomExamDto>(roomExam);
        }
        public async Task<bool> DeleteRoomExamAsync(Guid id)
        {
            return await _roomExamRepository.DeleteAsync(id);
        }
        public async Task<bool> UpdateRoomExamAsync(RoomExamDto roomExamDto)
        {
            if (roomExamDto == null) return false;
            var roomExam = _mapper.Map<RoomExam>(roomExamDto);
            return await _roomExamRepository.UpdateAsync(roomExam);
        }
    }
}
