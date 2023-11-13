using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class QuizItemResponseDto
    {
        [NotNull]
        public string? Nickname { get; set; }

        [NotNull]
        public bool IsScored { get; set; }

        [NotNull]
        public int TimeLimit { get; set; }

        [NotNull]
        public DateTime Started_At { get; set; }
    }
}
