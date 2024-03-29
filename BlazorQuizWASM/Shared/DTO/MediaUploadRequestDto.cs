﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Shared.DTO
{
    public class MediaUploadRequestDto
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}
