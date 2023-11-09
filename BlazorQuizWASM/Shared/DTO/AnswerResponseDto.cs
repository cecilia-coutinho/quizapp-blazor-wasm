using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQuizWASM.Shared.DTO
{
    public class AnswerResponseDto
    {
        public List<AnswerRequestDto>? Answers { get; set; }
    }
}
