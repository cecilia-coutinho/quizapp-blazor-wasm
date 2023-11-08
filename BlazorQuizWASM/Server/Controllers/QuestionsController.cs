using AutoMapper;
using BlazorQuizWASM.Server.CustomActionFilters;
using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(ApplicationDbContext context, IMapper mapper, IQuestionRepository questionRepository)
        {
            _context = context;
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        // CREATE Question
        //  POST: api/questions
        [HttpPost]
        [ValidateModel]
        //[Authorize]
        public async Task<ActionResult> CreateQuestion([FromBody] QuestionRequestDto questionRequestDto)
        {
            // Map DTO to Domain Model
            var questionDomainModel = _mapper.Map<Question>(questionRequestDto);

            await _questionRepository.CreateAsync(questionDomainModel);

            return Ok(_mapper
                .Map<QuestionRequestDto>(questionDomainModel));
        }

        // GET questions
        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult> GetAll(
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

            return Ok(_mapper
                .Map<List<QuestionRequestDto>>(questionDomainModel));
        }

        // GET questions by Id
        // GET: api/Questions/{id}
        [HttpGet("{id}")]
        [Route("{id:Guid}")]
        public async Task<ActionResult<Question>> GetQuestion(Guid id)
        {
            var questionDomainModel = await _questionRepository.GetByIdAsync(id);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(_mapper
                .Map<QuestionRequestDto>(questionDomainModel));
        }

        // UPDATE question By Id
        // PUT: api/Questions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, QuestionRequestDto questionRequestDto)
        {
            // Map DTO to Domain Model
            var questionDomainModel = _mapper.Map<Question>(questionRequestDto);

            questionDomainModel = await _questionRepository.UpdateAsync(id, questionDomainModel);

            if (questionDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(_mapper
                .Map<QuestionRequestDto>(questionDomainModel));
        }

        // DELETE question By Id
        // DELETE: api/Questions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var deleteQuestionDomainModel = await _questionRepository.DeleteAsync(id);

            if (deleteQuestionDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(_mapper
                .Map<QuestionRequestDto>(deleteQuestionDomainModel));
        }
    }
}
