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

        // GET Answers By Question
        // GET: api/Answers/answer-by-question
        [HttpGet]
        [ValidateModel]
        [Route("answer-by-question/{questionPath}")]
        [Authorize]
        public async Task<ActionResult> GetAnswersByQuestion(string questionPath)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get question id
            var question = await _questionRepository.GetQuestionByPathAndUserAsync(questionPath, userId);
            var questionId = question.QuestionId;

            var answers = await _answerRepository.GetAnswerToQuestionAsync(questionId);
            var response = answers.Select(a => new { a.Content, a.IsCorrect });

            return Ok(response);
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

        // DELETE Answer By Question and User (only a user can delete his own answers)
        // DELETE: api/Answers/delete
        [HttpDelete("delete")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer([FromForm] string questionTitle, [FromForm] string answer)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            var question = await _questionRepository.GetQuestionByPathAndUserAsync(questionTitle, userId);
            var questionId = question.QuestionId;

            var deleteAnswerDomainModel = await _answerRepository.DeleteAsync(answer, questionId);

            var answerDeleted = deleteAnswerDomainModel?.Content;

            return Ok(new { Answer = answerDeleted });
        }
    }
}
