using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class MediaUploadRequestDto
    {
        [Required]
        public string? MediaType { get; set; }

        [Required]
        public IFormFile? File { get; set; }

        [NotNull]
        public string? MediaFileName { get; set; }
    }
}
