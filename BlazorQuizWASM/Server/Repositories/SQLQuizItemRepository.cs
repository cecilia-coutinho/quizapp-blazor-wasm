using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Identity;
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

        public async Task<List<QuizItem>> GetScore(string fkUserId)
        {
            if (_context.QuizItems == null)
            {
                throw new Exception("Entity 'QuizItems' not found.");
            }

            var existingQuizItem = await _context.QuizItems
                .Where(x => x.FkUserId == fkUserId)
                .ToListAsync();

            return existingQuizItem;
        }

        public async Task<List<QuizItemQuestionResponseDto>> GetAllAsyncPerQuizCreator(string userId)
        {
            if (_context.QuizItems == null)
            {
                throw new Exception("Entity 'QuizItems' not found.");
            }

            var existingQuizItem = await _context.QuizItems
                .Join(_context.Users,
                    q => q.FkUserId,
                    quizUser => quizUser.Id,
                    (q, quizUser) => new { QuizItem = q, QuizUser = quizUser })
                .Join(_context.Questions,
                    joinResult => joinResult.QuizItem.FkQuestionId,
                    question => question.QuestionId,
                    (joinResult, question) => new { QuizItem = joinResult.QuizItem, QuizUser = joinResult.QuizUser, Question = question })
                .Where(result => result.Question.FkUserId == userId)
                .Select(result => new QuizItemQuestionResponseDto
                {
                    QuizItem = new QuizItemResponseDto
                    {
                        Nickname = result.QuizUser.Nickname,
                        IsScored = result.QuizItem.IsScored,
                        TimeSpent = result.QuizItem.TimeSpent,
                        Started_At = result.QuizItem.Started_At
                    },
                    QuestionPath = result.Question.QuestionPath,
                    QuestionTitle = result.Question.Title
                })
                .ToListAsync();

            return existingQuizItem;
        }
    }
}
