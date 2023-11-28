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

        public async Task<List<Answer>> CreateAsync(List<Answer> answers)
        {
            if (_context.Answers == null)
            {
                throw new Exception("Entity 'Answers' not found.");
            }
            foreach (var answer in answers)
            {
                await _context.Answers.AddAsync(answer);
            }
            await _context.SaveChangesAsync();
            return answers;
        }

        //public async Task<Answer?> DeleteAsync(string answer, Guid questionId)
        //{
        //    if (_context.Answers == null)
        //    {
        //        throw new Exception("Entity 'Answers' not found.");
        //    }

        //    var existingAnswer = await _context.Answers
        //        .Where(q => q.Content == answer && q.FkQuestionId == questionId)
        //        .FirstOrDefaultAsync();
        //    ;

        //    if (existingAnswer == null)
        //    {
        //        return null;
        //    }

        //    _context.Answers.Remove(existingAnswer);
        //    await _context.SaveChangesAsync();
        //    return existingAnswer;
        //}

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
                    Content = a.Content,
                    IsCorrect = a.IsCorrect
                })
                .ToListAsync();

            return existingAnswer;
        }
    }
}
