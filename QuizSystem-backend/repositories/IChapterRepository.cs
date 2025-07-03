using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IChapterRepository
    {
        Task<Chapter?>GetChapterByNameAsync(string chapterName);
    }
}
