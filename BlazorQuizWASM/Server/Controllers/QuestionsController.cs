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
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly IAnswerRepository _answerRepository;

        public QuestionsController(IQuestionRepository questionRepository, IMediaFileRepository mediaFileRepository, IAnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _mediaFileRepository = mediaFileRepository;
            _answerRepository = answerRepository;
        }

        // GET questions
        // GET: api/Questions/
        [HttpGet]
        public async Task<ActionResult> GetAllPublishedQuestionsWithAnswers(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000
            )
        {
            var questionDomainModel = await _questionRepository
                .GetAllAsync(
                filterOn,
                filterQuery,
                sortBy,
                isAscending ?? true,
                pageNumber,
                pageSize);

            var questions = questionDomainModel
              .Where(q => q.IsPublished == true)
              .Select(q => new
              {
                  Title = q.Title,
                  QuestionPath = q.QuestionPath,
                  FkQuestionId = q.QuestionId,
                  FkFileId = q.FkFileId,
                  TimeLimit = q.TimeLimit,
                  IsPublished = q.IsPublished
              })
              .ToList();

            var answersQuestionResponseDtoList = new List<AnswersQuestionResponseDto>();

            foreach (var question in questions)
            {

                var media = await _mediaFileRepository.GetMediaPath(question.FkFileId);

                var answers = await _answerRepository.GetAnswerToQuestionAsync(question.FkQuestionId);

                var answersQuestionResponseDto = new AnswersQuestionResponseDto
                {
                    Question = new QuestionResponseDto
                    {
                        Title = question.Title,
                        QuestionPath = question.QuestionPath,
                        MediaFileName = media?.MediaFileName,
                        MediaFilePath = media?.FilePath,
                        TimeLimit = question.TimeLimit,
                        IsPublished = question.IsPublished
                    },
                    Answers = answers.Select(a => new AnswerRequestDto
                    {
                        Content = a.Content,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };

                answersQuestionResponseDtoList.Add(answersQuestionResponseDto);
            }
            return Ok(answersQuestionResponseDtoList);
        }

        // GET questions with answers
        // GET: api/Questions/questions
        [HttpGet("questions-with-answers")]
        public async Task<ActionResult> GetAllQuestionsWithAnswers(
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
              .Select(q => new
              {
                  Title = q.Title,
                  IsPublished = q.IsPublished,
                  QuestionPath = q.QuestionPath,
                  FkQuestionId = q.QuestionId,
                  TimeLimit = q.TimeLimit
              })
              .ToList();

            var answersQuestionResponseDtoList = new List<AnswersQuestionResponseDto>();

            foreach (var question in questions)
            {
                var answers = await _answerRepository.GetAnswerToQuestionAsync(question.FkQuestionId);

                var answersQuestionResponseDto = new AnswersQuestionResponseDto
                {
                    Question = new QuestionResponseDto
                    {
                        Title = question.Title,
                        IsPublished = question.IsPublished,
                        QuestionPath = question.QuestionPath,
                    },
                    Answers = answers.Select(a => new AnswerRequestDto
                    {
                        Content = a.Content,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };

                answersQuestionResponseDtoList.Add(answersQuestionResponseDto);
            }
            return Ok(answersQuestionResponseDtoList);
        }

        // GET questions by path
        // GET: api/Questions/question/{questionPath}
        [HttpGet("question/{questionPath}")]
        public async Task<ActionResult> GetQuestionWithAnswers(string questionPath)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var question = await _questionRepository.GetQuestionByPathAndUserAsync(questionPath, userId);
            var questionId = question.QuestionId;

            var answers = await _answerRepository.GetAnswerToQuestionAsync(questionId);
            var response = answers.Select(a => new { a.Content, a.IsCorrect });

            var questionDomainModel = await _questionRepository.GetByIdAsync(questionId);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var questionTitle = questionDomainModel.Title;

            return Ok(new { Question = questionTitle });
        }

        // CREATE Question
        // POST: api/Questions/upload
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> CreateQuestion([FromBody] QuestionRequestDto questionRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get uploaded media file
            var mediaEntity = await _mediaFileRepository.GetMedia(questionRequestDto.MediaFileName);
            if (mediaEntity == null)
            {
                return Problem("Media Entity not found", statusCode: 500);
            }

            var questionpath = questionRequestDto.QuestionPath.ToLower().Replace(" ", "-");

            // Create the question
            Question question = new()
            {
                FkUserId = userId,
                FkFileId = mediaEntity.MediaFileId,
                Title = questionRequestDto.Title,
                QuestionPath = questionpath,
                TimeLimit = questionRequestDto.TimeLimit,
                IsPublished = questionRequestDto.IsPublished
            };

            await _questionRepository.CreateAsync(question);

            // map properties
            return Ok(new QuestionRequestDto
            {
                Title = question.Title,
                MediaFileName = questionRequestDto.MediaFileName,
                QuestionPath = questionRequestDto.QuestionPath,
                TimeLimit = questionRequestDto.TimeLimit,
                IsPublished = questionRequestDto.IsPublished
            });
        }

        // UPDATE question to publish
        // PUT: api/Questions/question/{id}
        [HttpPut("question/{questionPath}")]
        public async Task<IActionResult> Update(string questionPath)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //get question Id
            var requestedQuestion = await _questionRepository.GetQuestionByPath(questionPath);
            var questionId = requestedQuestion.QuestionId;

            Question question = new()
            {
                Title = requestedQuestion.Title,
                FkUserId = userId,
                FkFileId = requestedQuestion.FkFileId,
                QuestionPath = requestedQuestion.QuestionPath,
                TimeLimit = requestedQuestion.TimeLimit,
                IsPublished = true
            };

            var questionDomainModel = await _questionRepository.UpdateAsync(questionId, question);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            var updatedQuestionDto = new QuestionRequestDto
            {
                Title = questionDomainModel.Title,
                QuestionPath = questionDomainModel.QuestionPath,
                IsPublished = questionDomainModel.IsPublished
            };

            return Ok(updatedQuestionDto);
        }

        // DELETE question By Id
        // DELETE: api/Questions/question/{questionPath}
        [HttpDelete("question/{questionPath}")]
        public async Task<IActionResult> Delete(string questionPath)
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

        //// GET questions
        //// GET: api/Questions/questions-by-user
        //[HttpGet]
        //[Route("questions-by-user")]
        //public async Task<ActionResult> GetAllByUser(
        //    [FromQuery] string? filterOn,
        //    [FromQuery] string? filterQuery,
        //    [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
        //    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000
        //    )
        //{

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var questionDomainModel = await _questionRepository
        //        .GetAllAsync(
        //        filterOn,
        //        filterQuery,
        //        sortBy,
        //        isAscending ?? true,
        //        pageNumber,
        //        pageSize);

        //    var questions = questionDomainModel
        //    .Where(q => q.FkUserId == userId)
        //    .Select(q => new
        //    {
        //        Title = q.Title,
        //        IsPublished = q.IsPublished,
        //        QuestionPath = q.QuestionPath
        //    })
        //    .ToList();

        //    return Ok(new { Question = questions });
        //}



        // UPDATE question By Id
        // PUT: api/Questions/question/{id}
        //[HttpPut("question/{questionPath}")]
        //public async Task<IActionResult> Update(string questionPath, [FromForm] QuestionRequestDto questionRequestDto)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    //get question Id
        //    var requestedQuestion = await _questionRepository.GetQuestionByPath(questionPath);
        //    var questionId = requestedQuestion.QuestionId;

        //    // Get uploaded media file
        //    var mediaEntity = await _mediaFileRepository.GetMedia(questionRequestDto.MediaFileName);
        //    if (mediaEntity == null)
        //    {
        //        return Problem("Media Entity not found", statusCode: 500);
        //    }

        //    Question question = new()
        //    {
        //        Title = questionRequestDto.Title,
        //        FkUserId = userId,
        //        FkFileId = mediaEntity.MediaFileId,
        //        QuestionPath = questionRequestDto.QuestionPath,
        //        TimeLimit = questionRequestDto.TimeLimit,
        //        IsPublished = questionRequestDto.IsPublished
        //    };

        //    var questionDomainModel = await _questionRepository.UpdateAsync(questionId, question);

        //    if (questionDomainModel == null)
        //    {
        //        return NotFound();
        //    }

        //    var updatedQuestionDto = new QuestionRequestDto
        //    {
        //        Title = questionRequestDto.Title,
        //        MediaFileName = questionRequestDto.MediaFileName,
        //        QuestionPath = questionRequestDto.QuestionPath,
        //        TimeLimit = questionRequestDto.TimeLimit,
        //        IsPublished = questionRequestDto.IsPublished
        //    };

        //    return Ok(updatedQuestionDto);
        //}
    }
}
