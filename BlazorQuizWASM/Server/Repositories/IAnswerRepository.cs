using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> CreateAsync(List<Answer> answers);
        Task<List<Answer>> GetAnswerToQuestionAsync(Guid fkQuestionId);
    }
}
