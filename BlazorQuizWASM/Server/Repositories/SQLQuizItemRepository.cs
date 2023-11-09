using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Repositories
{
    public class SQLQuizItemRepository : IQuizItemRepository
    {
        private readonly ApplicationDbContext _context;

        public SQLQuizItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<QuizItem> CreateAsync(QuizItem quizItem)
        {
            if (_context.QuizItems == null)
            {
                throw new Exception("Entity 'QuizItems' not found.");
            }

            await _context.QuizItems.AddAsync(quizItem);
            await _context.SaveChangesAsync();
            return quizItem;
        }

        public async Task<List<QuizItemRequestDto>> GetParticipantsPerQuestionAsync(Guid fkQuestionId)
        {
            if (_context.QuizItems == null)
            {
                throw new Exception("Entity 'QuizItems' not found.");
            }

            var existingQuizItem = await _context.QuizItems
                .Where(x => x.FkQuestionId == fkQuestionId)
                .Join(_context.Users,
                q => q.FkUserId,
                u => u.Id,
                (q, u) => new QuizItemRequestDto
                {
                    Nickname = u.Nickname,
                    IsScored = q.IsScored,
                    TimeLimit = q.TimeLimit,
                    Started_At = q.Started_At
                })
                .ToListAsync();

            return existingQuizItem;
        }
    }
}
