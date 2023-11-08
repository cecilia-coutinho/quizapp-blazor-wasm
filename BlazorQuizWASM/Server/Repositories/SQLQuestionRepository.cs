using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Repositories
{
    public class SQLQuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public SQLQuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(Question question)
        {
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question?> DeleteAsync(Guid id)
        {
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var existingQuestion = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == id);

            if (existingQuestion == null)
            {
                return null;
            }

            _context.Questions.Remove(existingQuestion);
            await _context.SaveChangesAsync();
            return existingQuestion;
        }

        public async Task<List<Question>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var questions = _context?.Questions?.Include("ApplicationUser").Include("MediaFile").AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    questions = questions?.Where(x => x.Title.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    questions = isAscending ? questions?.OrderBy(x => x.Title) : questions?.OrderByDescending(x => x.Title);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            if (questions == null)
            {
                throw new Exception("No questions found.");
            }

            return await questions.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Question> GetByIdAsync(Guid id)
        {
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var question = await _context.Questions
                .Include("ApplicationUser")
                .Include("MediaFile")
                .FirstOrDefaultAsync(x => x.QuestionId == id);

            if (question == null)
            {
                throw new Exception("Question not found.");
            }

            return question;
        }

        public async Task<Question?> UpdateAsync(Guid id, Question question)
        {
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var existingQuestion = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == id);

            if (existingQuestion == null)
            {
                return null;
            }

            existingQuestion.Title = question.Title;
            existingQuestion.Description = question.Description;
            existingQuestion.FkUserId = question.FkUserId;
            existingQuestion.FkFileId = question.FkFileId;

            await _context.SaveChangesAsync();
            return existingQuestion;
        }
    }
}
