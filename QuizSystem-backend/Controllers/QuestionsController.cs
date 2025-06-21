using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        //private readonly QuizSystemDbContext _context;
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult> GetQuestions()
        {
            try
            {
                var result = await _questionService.GetQuestionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetQuestion(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid question ID." });
                }

                var question = await _questionService.GetQuestionByIdAsync(id);

                if (question == null)
                {
                    return NotFound(new { message = $"Question with ID {id} not found." });
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutQuestion(Guid id, [FromBody] QuestionDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(id, dto);
                if (updatedQuestion == null)
                {
                    return NotFound(new { message = $"Question with ID {id} not found." });
                }

                return Ok(updatedQuestion);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, new { message = "Concurrency conflict occurred while updating." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }


        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostQuestion([FromBody] QuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            try
            {
                var newQuestion = await _questionService.AddQuestionAsync(dto);

                return Ok(newQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }


        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                bool isDeleted = await _questionService.DeleteQuestionAsync(id);

                if(!isDeleted)
                {
                    return NotFound();
                }

                return Ok(new { message = "Deleted Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
    }
}
