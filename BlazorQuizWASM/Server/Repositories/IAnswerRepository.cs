using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> CreateAsync(Answer answer);
        Task<List<Answer>> GetAnswerToQuestionAsync(Guid fkQuestionId);
        Task<Answer?> DeleteAsync(string answer, Guid questionId);
    }
}
