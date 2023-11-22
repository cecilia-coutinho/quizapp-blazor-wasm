using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorQuizWASM.Shared.DTO
{
    public class AnswersQuestionResponseDto
    {
        public QuestionRequestDto? Question { get; set; }
        public List<AnswerRequestDto>? Answers { get; set; }
    }
}
