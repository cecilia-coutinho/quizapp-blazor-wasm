using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlazorQuizWASM.Shared.DTO;
using BlazorQuizWASM.Server.CustomActionFilters;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuizItemRepository _quizItemRepository;

        public QuizItemsController(ApplicationDbContext context, IQuizItemRepository quizItemRepository)
        {
            _context = context;
            _quizItemRepository = quizItemRepository;
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
        [Route("participants")]
        [Authorize]
        public async Task<ActionResult<QuizItem>> GetParticipantsPerQuestion([FromForm] UpdateQuestionRequestDto updateQuestionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get Question Id
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Title == updateQuestionRequestDto.QuestionPath);

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
        public async Task<ActionResult<QuizItem>> Upload([FromForm] QuizItemRequestDto quizItemRequestDto, [FromForm] UpdateQuestionRequestDto updateQuestionRequestDto )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get Question Id
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Title == updateQuestionRequestDto.QuestionPath);

            if (question == null)
            {
                return NotFound("This question does not exist. Please check your input data and try again.");
            }

            QuizItem quizItem = new()
            {
                IsScored = quizItemRequestDto.IsScored,
                TimeSpent = quizItemRequestDto.TimeSpent,
                Started_At = quizItemRequestDto.Started_At,
                FkUserId = userId,
                FkQuestionId = question.QuestionId
            };

            await _quizItemRepository.CreateAsync(quizItem);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return Ok( new QuizItemResponseDto 
            {
                Nickname = user?.Nickname,
                IsScored = quizItem.IsScored,
                TimeSpent = quizItem.TimeSpent,
                Started_At = quizItem.Started_At
            });
        }
    }
}
