using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class QuestionRequestDto
    {
        [NotNull]
        public string? MediaFileName { get; set; }

        [NotNull]
        public string? Title { get; set; }

        [NotNull]
        public string? Content { get; set; }

        [NotNull]
        public bool IsCorrect { get; set; } = true;
    }
}
