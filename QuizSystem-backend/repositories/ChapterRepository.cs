using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly QuizSystemDbContext _context;

        public ChapterRepository(QuizSystemDbContext context)
        {
            _context = context;
        }

        

        public async Task<Chapter?> GetChapterByNameAsync(string chapterName)
        {
            if (string.IsNullOrEmpty(chapterName))
            {
                return null; // or throw an exception
            }
            var chapter = await _context.Chapters
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Name == chapterName);
            return chapter!;
        }
    }
}
