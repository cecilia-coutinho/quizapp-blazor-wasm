using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IQuizItemRepository
    {
        Task<QuizItem> CreateAsync(QuizItem quizItem);
        //Task<List<QuizItemResponseDto>> GetParticipantsPerQuestionAsync(Guid fkQuestionId);
        Task<List<QuizItem>> GetScore(string fkUserId);
        Task<List<QuizItemQuestionResponseDto>> GetAllAsyncPerQuizCreator(string userId);
    }
}
