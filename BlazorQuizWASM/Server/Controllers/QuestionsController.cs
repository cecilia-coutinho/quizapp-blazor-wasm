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
            if (mediaEntity == null)
            {
              return Problem( "Media Entity not found", statusCode: 500);
            }

            // Create the question
            Question question = new()
            {
                FkUserId = userId,
                FkFileId = mediaEntity.MediaFileId,
                Title = questionRequestDto.Title,
                QuestionPath = questionRequestDto.QuestionPath,
                TimeLimit = questionRequestDto.TimeLimit
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
            var questionId = question.QuestionId;

            var questionDomainModel = await _questionRepository.GetByIdAsync(questionId);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var questionTitle = questionDomainModel.Title;

            return Ok(new { Question = questionTitle });
        }

        // UPDATE question By Id
        // PUT: api/Questions/question/{id}
        [HttpPut("question/{questionPath}")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update(string questionPath, [FromForm] QuestionRequestDto questionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //get question Id
            var requestedQuestion = await _questionRepository.GetQuestionByPath(questionPath);
            var questionId = requestedQuestion.QuestionId;

            // Get uploaded media file
            var mediaEntity = await _mediaFileRepository.GetMedia(questionRequestDto.MediaFileName);
            if (mediaEntity == null)
            {
                return Problem("Media Entity not found", statusCode: 500);
            }

            Question question = new()
            {
                Title = questionRequestDto.Title,
                FkUserId = userId,
                FkFileId = mediaEntity.MediaFileId,
                QuestionPath = questionRequestDto.QuestionPath,
                TimeLimit = questionRequestDto.TimeLimit
            };

            var questionDomainModel = await _questionRepository.UpdateAsync(questionId, question);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var updatedQuestionDto = new QuestionRequestDto
            {
                Title = questionRequestDto.Title,
                MediaFileName = questionRequestDto.MediaFileName,
                QuestionPath = questionRequestDto.QuestionPath,
                TimeLimit = questionRequestDto.TimeLimit
            };

            return Ok(updatedQuestionDto);
        }

        // DELETE question By Id
        // DELETE: api/Questions/question/{id}
        [HttpDelete("question/{questionPath}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] string questionPath)
        {
            //get question Id
            var requestedQuestion = await _questionRepository.GetQuestionByPath(questionPath);
            var questionId = requestedQuestion.QuestionId;

            var deleteQuestionDomainModel = await _questionRepository.DeleteAsync(questionId);

            if (deleteQuestionDomainModel == null)
            {
                return NotFound();
            }

            var question = deleteQuestionDomainModel.Title;

            return Ok(new { Question = question });
        }
    }
}
