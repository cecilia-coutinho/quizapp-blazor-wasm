using AutoMapper;
using Azure.Core;
using BlazorQuizWASM.Server.CustomActionFilters;
using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(ApplicationDbContext context, IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
            _context = context;
            _questionRepository = questionRepository;
        }

        // CREATE Question
        // POST: api/Questions/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> CreateQuestion([FromForm] QuestionRequestDto questionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get uploaded media file
            if (_context.MediaFiles == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var mediaEntity = await _context.MediaFiles.FirstOrDefaultAsync(mediaFile => mediaFile.MediaFileName == questionRequestDto.MediaFileName);

            // Create the question
            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            Question question = new()
            {
                Title = questionRequestDto.Title,
                FkUserId = userId,
                FkFileId = mediaEntity?.MediaFileId ?? throw new ArgumentNullException(nameof(mediaEntity), "mediaEntity cannot be null.")
            };

            await _questionRepository.CreateAsync(question);

            // map properties
            return Ok(new QuestionRequestDto
            {
                Title = question.Title,
                MediaFileName = questionRequestDto.MediaFileName,
                Content = questionRequestDto.Content
            });

        }

        // GET questions
        // GET: api/Questions/questions-by-user
        [HttpGet]
        [Route("questions-by-user")]
        [Authorize]
        public async Task<ActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000
            )
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var questionDomainModel = await _questionRepository
                .GetAllAsync(
                filterOn,
                filterQuery,
                sortBy,
                isAscending ?? true,
                pageNumber,
                pageSize);

            var questions = questionDomainModel
            .Where(q => q.FkUserId == userId)
            .Select(q => q.Title)
            .ToList();

            return Ok(new {Question = questions});
        }


        // GET questions by Id
        // GET: api/Questions/question/{id}
        [HttpGet("question/{id}")]
        public async Task<ActionResult> GetQuestion(Guid id)
        {
            var questionDomainModel = await _questionRepository.GetByIdAsync(id);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var question = questionDomainModel.Title;

            return Ok(new { Question = question });
        }

        // UPDATE question By Id
        // PUT: api/Questions/question/{id}
        [HttpPut("question/{id}")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] QuestionRequestDto questionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get uploaded media file
            if (_context.MediaFiles == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            var mediaEntity = await _context.MediaFiles.FirstOrDefaultAsync(mediaFile => mediaFile.MediaFileName == questionRequestDto.MediaFileName);

            if (_context.Questions == null)
            {
                throw new Exception("Entity 'Questions' not found.");
            }

            Question question = new()
            {
                Title = questionRequestDto.Title,
                FkUserId = userId,
                FkFileId = mediaEntity?.MediaFileId ?? throw new ArgumentNullException(nameof(mediaEntity), "mediaEntity cannot be null.")
            };

            var questionDomainModel = await _questionRepository.UpdateAsync(id, question);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var updatedQuestionDto = new QuestionRequestDto
            {
                Title = questionRequestDto.Title,
                MediaFileName = questionRequestDto.MediaFileName,
                Content = questionRequestDto.Content
            };

            return Ok(updatedQuestionDto);
        }

        // DELETE question By Id
        // DELETE: api/Questions/question/{id}
        [HttpDelete("question/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteQuestionDomainModel = await _questionRepository.DeleteAsync(id);

            if (deleteQuestionDomainModel == null)
            {
                return NotFound();
            }

            var question = deleteQuestionDomainModel.Title;

            return Ok(new { Question = question });
        }
    }
}
