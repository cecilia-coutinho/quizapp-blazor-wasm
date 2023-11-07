using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQuizWASM.Shared.DTO
{
    public class MediaFileResponseDto
    {
        [NotNull]
        public string? MediaFileName { get; set; }

        public string? FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string? FilePath { get; set; }
    }
}
