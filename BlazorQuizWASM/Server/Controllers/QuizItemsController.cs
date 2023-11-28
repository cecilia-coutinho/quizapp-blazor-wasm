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
    [Authorize]
    [ValidateModel]
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

        // GET: api/QuizItems
        [HttpGet]
        public async Task<ActionResult> GetParticipantsPerQuestion()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Problem(detail: "Could not fetch user", statusCode: 500);
            }

            var quizItem = await _quizItemRepository.GetAllAsyncPerQuizCreator(userId);
            var quizItemResponseList = new List<QuizItemQuestionResponseDto>();

            foreach (var item in quizItem)
            {
                if (item.QuizItem == null)
                {
                    return NotFound("This question does not have any participants.");
                }

                var quizItemQuestionResponseDto = new QuizItemQuestionResponseDto
                {
                    QuizItem = new QuizItemResponseDto
                    {
                        Nickname = item.QuizItem.Nickname,
                        IsScored = item.QuizItem.IsScored,
                        TimeSpent = item.QuizItem.TimeSpent,
                        Started_At = item.QuizItem.Started_At
                    },
                    QuestionPath = item.QuestionPath,
                    QuestionTitle = item.QuestionTitle
                };

                quizItemResponseList.Add(quizItemQuestionResponseDto);
            }

            return Ok(quizItemResponseList);
        }

        // GET: api/QuizItems/score
        [HttpGet]
        [Route("score")]
        public async Task<ActionResult> GetScore()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Problem(detail: "Could not fetch user", statusCode: 500);
            }

            var scores = await _quizItemRepository.GetScore(userId);

            var filteredScores = scores.Where(score => score.IsScored == true).ToList();

            var userScore = filteredScores.Count();

            return Ok(userScore);
        }

        // POST: api/QuizItems/upload
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> Upload([FromBody] QuizItemQuestionResquestDto request)
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
