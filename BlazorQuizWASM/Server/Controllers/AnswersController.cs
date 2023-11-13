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
        // POST: api/Answers/answer-by-question
        [HttpPost]
        [ValidateModel]
        [Route("answer-by-question")]
        [Authorize]
        public async Task<ActionResult> GetAnswersByQuestionPost([FromForm] UpdateQuestionRequestDto questionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get question id
            var question = await _questionRepository.GetQuestionByTitleAndUserAsync(questionRequestDto.Title, userId);
            var questionId = question.QuestionId;

            var answers = await _answerRepository.GetAnswerToQuestionAsync(questionId);
            var response = answers.Select(a => new { a.Content, a.IsCorrect });

            return Ok(response);
        }

        // CREATE Answer
        // POST: api/Answers/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> PostAnswer([FromForm] UpdateQuestionRequestDto questionRequestDto, [FromForm] AnswerRequestDto answerRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            var question = await _questionRepository.GetQuestionByTitleAndUserAsync(questionRequestDto.Title, userId);
            var questionId = question.QuestionId;

            var answer = new Answer
            {
                Content = answerRequestDto.Content,
                IsCorrect = answerRequestDto.IsCorrect,
                FkQuestionId = questionId
            };

            await _answerRepository.CreateAsync(answer);

            return Ok(new AnswerRequestDto
            {
                Content = answer.Content,
                IsCorrect = answer.IsCorrect
            });
        }

        // DELETE Answer By Question and User (only a user can delete his own answers)
        // DELETE: api/Answers/delete
        [HttpDelete("delete")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer([FromForm] string questionRequest, [FromForm] string answer)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            var question = await _questionRepository.GetQuestionByTitleAndUserAsync(questionRequest, userId);
            var questionId = question.QuestionId;

            var deleteAnswerDomainModel = await _answerRepository.DeleteAsync(answer, questionId);

            var answerDeleted = deleteAnswerDomainModel?.Content;

            return Ok(new { Answer = answerDeleted });
        }
    }
}
