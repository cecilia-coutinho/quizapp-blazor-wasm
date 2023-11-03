using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorQuizWASM.Server.Models.Domain
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }
        
        [Required]
        public Guid FkUserId { get; set; }

        [Required]
        public Guid FkFilesId { get; set; }

        [NotNull]
        public string? Title { get; set; }

        public string? Description { get; set; }


        // Navigation properties
        [ForeignKey("FkUserId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }

        [ForeignKey("FkFilesId")]
        public virtual MediaFile? MediaFiles { get; set; }
        public List<Answer>? Answers { get; set; }
        public List<QuizItem>? QuizItems { get; set; }
    }
}
