﻿using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IQuizItemRepository
    {
        Task<QuizItem> CreateAsync(QuizItem quizItem);
        Task<List<QuizItemRequestDto>> GetParticipantsPerQuestionAsync(Guid fkQuestionId);
    }
}
