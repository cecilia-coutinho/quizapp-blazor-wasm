﻿using AutoMapper;
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
        //private readonly IQuestionRepository _questionRepository;
        //private readonly MediaFilesController _mediaFilesController;

        public QuestionsController(ApplicationDbContext context)
        {
            //_questionRepository = questionRepository;
            //_mediaFilesController = mediaFilesController;
            _context = context;
        }

        // CREATE Question
        // POST: api/Questions/AddQuestion
        [HttpPost]
        [Route("Upload")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> CreateQuestion([FromForm] QuestionRequestDto questionRequestDto)
        {
            //Get user id
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

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            // map properties
            return Ok(new QuestionRequestDto
            {
                Title = question.Title,
                MediaFileName = questionRequestDto.MediaFileName,
                Content = questionRequestDto.Content
            });

        }

        //// GET questions
        //// GET: api/Questions/GetAll
        //[HttpGet]
        //[Route("GetAll")]
        //public async Task<ActionResult> GetAll(
        //    [FromQuery] string? filterOn,
        //    [FromQuery] string? filterQuery,
        //    [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
        //    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000
        //    )
        //{
        //    var questionDomainModel = await _questionRepository
        //        .GetAllAsync(
        //        filterOn,
        //        filterQuery,
        //        sortBy,
        //        isAscending ?? true,
        //        pageNumber,
        //        pageSize);

        //    return Ok(_mapper
        //        .Map<List<QuestionRequestDto>>(questionDomainModel));
        //}

        //// GET questions by Id
        //// GET: api/Questions/{id}
        //[HttpGet("{id}")]
        //[Route("{id:Guid}")]
        //public async Task<ActionResult> GetQuestion(Guid id)
        //{
        //    var questionDomainModel = await _questionRepository.GetByIdAsync(id);

        //    if (questionDomainModel == null)
        //    {
        //        return NotFound();
        //    }

        //    // Map Domain Model to DTO
        //    return Ok(_mapper
        //        .Map<QuestionRequestDto>(questionDomainModel));
        //}

        //// UPDATE question By Id
        //// PUT: api/Questions/{id}
        //[HttpPut]
        //[Route("{id:Guid}")]
        //[ValidateModel]
        //public async Task<IActionResult> Update([FromRoute] Guid id, QuestionRequestDto questionRequestDto)
        //{
        //    // Map DTO to Domain Model
        //    var questionDomainModel = _mapper.Map<Question>(questionRequestDto);

        //    questionDomainModel = await _questionRepository.UpdateAsync(id, questionDomainModel);

        //    if (questionDomainModel == null)
        //    {
        //        return NotFound();
        //    }

        //    // Map Domain Model to DTO
        //    return Ok(_mapper
        //        .Map<QuestionRequestDto>(questionDomainModel));
        //}

        //// DELETE question By Id
        //// DELETE: api/Questions/{id}
        //[HttpDelete]
        //[Route("{id:Guid}")]
        //public async Task<IActionResult> Delete([FromRoute] Guid id)
        //{
        //    var deleteQuestionDomainModel = await _questionRepository.DeleteAsync(id);

        //    if (deleteQuestionDomainModel == null)
        //    {
        //        return NotFound();
        //    }

        //    // Map Domain Model to DTO
        //    return Ok(_mapper
        //        .Map<QuestionRequestDto>(deleteQuestionDomainModel));
        //}
    }
}