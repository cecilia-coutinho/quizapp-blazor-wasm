using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQuizWASM.Shared.DTO
{
    public class AnswersQuestionRequestDto
    {
        public string? Path { get; set; }
        public List<AnswerRequestDto>? Answers { get; set; }
    }
}
