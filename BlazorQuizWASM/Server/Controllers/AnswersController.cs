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
        private readonly ApplicationDbContext _context;
        private readonly IAnswerRepository _answerRepository;

        public AnswersController(ApplicationDbContext context, IAnswerRepository answerRepository)
        {
            _context = context;
            _answerRepository = answerRepository;
        }

        // GET Answers By Question
        // POST: api/Answers/answer-by-question
        [HttpPost]
        [ValidateModel]
        [Route("answer-by-question")]
        [Authorize]
        public async Task<ActionResult> GetAnswersByQuestionPost([FromForm] QuestionResponseDto question)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get question id
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var questionId = await _context.Questions
                .Where(q => q.Title == question.Title && q.FkUserId == userId)
                .Select(q => q.QuestionId)
                .FirstOrDefaultAsync();

            var answers = await _answerRepository.GetAnswerToQuestionAsync(questionId);

            var response = answers.Select(a => new { Content = a.Content, IsCorrect = a.IsCorrect });

            return Ok(response);
        }

        // CREATE Answer
        // POST: api/Answers/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> PostAnswer([FromForm] QuestionResponseDto question, [FromForm] AnswerRequestDto answerRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var questionId = await _context.Questions
                .Where(q => q.Title == question.Title && q.FkUserId == userId)
                .Select(q => q.QuestionId)
                .FirstOrDefaultAsync();

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
        public async Task<IActionResult> DeleteAnswer([FromForm] string question, [FromForm] string answer)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get question id
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var questionId = await _context.Questions
                .Where(q => q.Title == question && q.FkUserId == userId)
                .Select(q => q.QuestionId)
                .FirstOrDefaultAsync();

            var deleteAnswerDomainModel = await _answerRepository.DeleteAsync(answer, questionId);

            if (deleteAnswerDomainModel == null)
            {
                return NotFound();
            }

            var answerDeleted = deleteAnswerDomainModel.Content;

            return Ok(new { Answer = answerDeleted });
        }
    }
}
