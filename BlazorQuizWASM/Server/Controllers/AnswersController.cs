using BlazorQuizWASM.Server.CustomActionFilters;
using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;

        public AnswersController(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
        }

        // CREATE Answers
        // POST: api/Answers/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnswerRequestDto>>> PostAnswer([FromBody] AnswersQuestionRequestDto answerRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            var question = await _questionRepository.GetQuestionByPathAndUserAsync(answerRequestDto.Path, userId);
            var questionId = question.QuestionId;

            var answers = new List<Answer>();
            {
                if (answerRequestDto.Answers == null)
                {
                    return Problem("No answers found", statusCode: 500);
                }

                foreach (var answer in answerRequestDto.Answers)
                {
                    answers.Add(new Answer
                    {
                        Content = answer.Content,
                        IsCorrect = answer.IsCorrect,
                        FkQuestionId = questionId
                    });
                }
            };

            await _answerRepository.CreateAsync(answers);

            answerRequestDto.Answers.Select(a => new AnswerRequestDto
            {
                Content = a.Content,
                IsCorrect = a.IsCorrect
            });

            return Ok(answerRequestDto);
        }
    }
}
