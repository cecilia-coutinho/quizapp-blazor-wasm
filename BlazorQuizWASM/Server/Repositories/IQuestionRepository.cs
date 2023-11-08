using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> CreateAsync(Question question);
        Task<List<Question>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Question> GetByIdAsync(Guid id);
        Task<Question?> UpdateAsync(Guid id, Question question);
        Task<Question?> DeleteAsync(Guid id);
    }
}
