using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/QuizItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizItem>>> GetQuizItems()
        {
          if (_context.QuizItems == null)
          {
              return NotFound();
          }
            return await _context.QuizItems.ToListAsync();
        }

        // GET: api/QuizItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizItem>> GetQuizItem(Guid id)
        {
          if (_context.QuizItems == null)
          {
              return NotFound();
          }
            var quizItem = await _context.QuizItems.FindAsync(id);

            if (quizItem == null)
            {
                return NotFound();
            }

            return quizItem;
        }

        // PUT: api/QuizItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizItem(Guid id, QuizItem quizItem)
        {
            if (id != quizItem.QuizItemId)
            {
                return BadRequest();
            }

            _context.Entry(quizItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/QuizItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizItem>> PostQuizItem(QuizItem quizItem)
        {
          if (_context.QuizItems == null)
          {
              return Problem("Entity set 'ApplicationDbContext.QuizItems'  is null.");
          }
            _context.QuizItems.Add(quizItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizItem", new { id = quizItem.QuizItemId }, quizItem);
        }

        // DELETE: api/QuizItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizItem(Guid id)
        {
            if (_context.QuizItems == null)
            {
                return NotFound();
            }
            var quizItem = await _context.QuizItems.FindAsync(id);
            if (quizItem == null)
            {
                return NotFound();
            }

            _context.QuizItems.Remove(quizItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizItemExists(Guid id)
        {
            return (_context.QuizItems?.Any(e => e.QuizItemId == id)).GetValueOrDefault();
        }
    }
}
