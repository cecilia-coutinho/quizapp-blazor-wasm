using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public class SQLAnswerRepository : IAnswerRepository
    {
        Task<Answer> IAnswerRepository.CreateAsync(Answer answer)
        {
            throw new NotImplementedException();
        }

        Task<Answer?> IAnswerRepository.DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<List<Answer>> IAnswerRepository.GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAscending, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
