using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "varchar(25)")]
        public string? QuestionPath { get; set; }

        [NotNull]
        [Range(1, 45, ErrorMessage = "The time limit must be between 1 and 45min.")]
        public int TimeLimit { get; set; }

        [NotNull]
        public bool IsPublished { get; set; } = false;
    }
}
