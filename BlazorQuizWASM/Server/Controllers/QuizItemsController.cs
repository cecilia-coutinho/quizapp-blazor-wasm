using BlazorQuizWASM.Server.CustomActionFilters;
using BlazorQuizWASM.Server.Models;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizItemsController : ControllerBase
    {
        private readonly IQuizItemRepository _quizItemRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuizItemsController(IQuizItemRepository quizItemRepository, IQuestionRepository questionRepository, UserManager<ApplicationUser> userManager)
        {
            _quizItemRepository = quizItemRepository;
            _questionRepository = questionRepository;
            _userManager = userManager;
        }

        // GET: api/QuizItems/score
        [HttpGet]
        [Route("score")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<QuizItem>>> GetScore()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Problem(detail: "Could not fetch user", statusCode: 500);
            }

            var scores = await _quizItemRepository.GetScore(userId);

            foreach (var score in scores)
            {
                // get only isScored == true
                if (score.IsScored == false)
                {
                    scores.Remove(score);
                }
            }

            var userScore = scores.Count();

            return Ok(userScore);
        }

        // GET: api/QuizItems/participants
        [HttpGet]
        [Route("participants/{questionPath}")]
        [Authorize]
        public async Task<ActionResult<QuizItem>> GetParticipantsPerQuestion(string questionPath)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get Question Id
            var question = await _questionRepository.GetQuestionByPath(questionPath);

            if (question == null)
            {
                return NotFound("This question does not exist. Please check your input data and try again.");
            }

            var quizItem = await _quizItemRepository.GetParticipantsPerQuestionAsync(question.QuestionId);

            return Ok(quizItem);
        }

        // POST: api/QuizItems/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult<QuizItem>> Upload([FromForm] QuizItemQuestionResquestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (request.QuestionPath != null && request.QuizItem != null && userId != null)
            {
                // Get Question Id
                var question = await _questionRepository.GetQuestionByPath(request.QuestionPath);

                if (question == null)
                {
                    ModelState.AddModelError("QuestionPath", "Please provide a valid question path.");
                    return BadRequest(ModelState);
                }

                QuizItem quizItem = new()
                {
                    IsScored = request.QuizItem.IsScored,
                    TimeSpent = request.QuizItem.TimeSpent,
                    Started_At = request.QuizItem.Started_At,
                    FkUserId = userId,
                    FkQuestionId = question.QuestionId
                };

                await _quizItemRepository.CreateAsync(quizItem);

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ModelState.AddModelError("UserId", "User not found.");
                    return BadRequest(ModelState);
                }

                return Ok(new QuizItemResponseDto
                {
                    Nickname = user?.Nickname,
                    IsScored = quizItem.IsScored,
                    TimeSpent = quizItem.TimeSpent,
                    Started_At = quizItem.Started_At
                });
            }

            ModelState.AddModelError("Invalid data or user is not logged in", "Please provide a valid input data and ensure login validation.");
            return BadRequest(ModelState);
        }
    }
}
