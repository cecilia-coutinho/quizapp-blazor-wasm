using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Repositories
{
    public class SQLAnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public SQLAnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            if (_context.Answers == null)
            {
                throw new Exception("Entity 'Answers' not found.");
            }

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer?> DeleteAsync(Guid id)
        {
            if (_context.Answers == null)
            {
                throw new Exception("Entity 'Answers' not found.");
            }

            var existingAnswer = await _context.Answers.FirstOrDefaultAsync(x => x.AnswerId == id);

            if (existingAnswer == null)
            {
                return null;
            }

            _context.Answers.Remove(existingAnswer);
            await _context.SaveChangesAsync();
            return existingAnswer;
        }

        public async Task<List<Answer>> GetAnswerToQuestionAsync(Guid fkQuestionId)
        {
            if (_context.Answers == null)
            {
                throw new Exception("Entity 'Answers' not found.");
            }

            var existingAnswer = await _context.Answers
                .Where(x => x.FkQuestionId == fkQuestionId)
                .Select(a => new Answer
                {
                    Content= a.Content,
                    IsCorrect = a.IsCorrect
                })
                .ToListAsync();

            return existingAnswer;
        }
    }
}
