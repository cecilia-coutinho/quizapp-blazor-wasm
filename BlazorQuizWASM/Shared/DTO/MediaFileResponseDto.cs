using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class MediaFileResponseDto
    {
        [NotNull]
        public string? MediaFileName { get; set; }

        public string? StoredFileName { get; set; }
        public string? FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string? FilePath { get; set; }
        public bool Uploaded { get; set; }
        public int ErrorCode { get; set; }
    }
}
