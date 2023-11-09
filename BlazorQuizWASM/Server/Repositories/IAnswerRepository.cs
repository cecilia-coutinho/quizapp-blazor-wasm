using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> CreateAsync(Answer answer);
        Task<List<Answer>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Answer?> DeleteAsync(Guid id);
    }
}
