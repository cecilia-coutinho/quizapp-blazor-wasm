using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public Guid FkMediaTypeId { get; set; }

        [Required]
        public IFormFile? File { get; set; }

        [NotNull]
        public string? MediaFileName { get; set; }
    }
}
