using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQuizWASM.Shared.DTO
{
    public class QuestionRequestDto
    {
        //[Key]
        //public Guid QuestionId { get; set; }

        [Required]
        public string? FkUserId { get; set; }

        [Required]
        public Guid FkFileId { get; set; }

        [NotNull]
        public string? Title { get; set; }

        public string? Description { get; set; }


    }
}
