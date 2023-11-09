using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IQuizItemRepository
    {
        Task<QuizItem> CreateAsync(QuizItem quizItem);
        Task<List<QuizItem>> GetParticipantsPerQuestionAsync(Guid fkQuestionId);
    }
}
