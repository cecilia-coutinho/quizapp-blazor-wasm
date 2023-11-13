using BlazorQuizWASM.Server.CustomActionFilters;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediaFileRepository _mediaFileRepository;

        public QuestionsController(IQuestionRepository questionRepository, IMediaFileRepository mediaFileRepository)
        {
            _questionRepository = questionRepository;
            _mediaFileRepository = mediaFileRepository;
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
            var mediaEntity = await _mediaFileRepository.GetMedia(questionRequestDto.MediaFileName);

            // Create the question
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
                QuestionPath = questionRequestDto.QuestionPath,
                TimeLimit = questionRequestDto.TimeLimit
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

            return Ok(new { Question = questions });
        }


        // GET questions by Id
        // GET: api/Questions/question/{questionPath}
        [HttpGet("question/{questionPath}")]
        public async Task<ActionResult> GetQuestion(string questionPath)
        {
            var question = await _questionRepository.GetQuestionByPath(questionPath);

            var questionDomainModel = await _questionRepository.GetByIdAsync(question.QuestionId);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var questionTitle = questionDomainModel.Title;

            return Ok(new { Question = questionTitle });
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
            var mediaEntity = await _mediaFileRepository.GetMedia(questionRequestDto.MediaFileName);

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
                MediaFileName = questionRequestDto.MediaFileName
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
