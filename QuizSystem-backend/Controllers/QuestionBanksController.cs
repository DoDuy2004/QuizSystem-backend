using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "TEACHER, ADMIN")]
    public class QuestionBanksController : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService;

        public QuestionBanksController(IQuestionBankService questionBankService)
        {
            _questionBankService = questionBankService;
        }

        // GET: api/QuestionBanks
        [HttpGet]
        public async Task<ActionResult> GetQuestionBanks()
        {
            try
            {
                var questionBanks = await _questionBankService.GetQuestionBanksAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = questionBanks
                });
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //GET: api/QuestionBanks/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetQuestionBank(Guid id)
        {
            if(Guid.Empty == id)
            {
                return BadRequest();      
            }

            try
            {
                var questionBank = await _questionBankService.GetQuestionBankByIdAsync(id);

                if (questionBank == null)
                {
                    return NotFound(new { message = $"Question bank with ID {id} not found." });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = questionBank
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        [HttpGet("{id}/questions")]
        public async Task<ActionResult> GetQuestionsByQuestionBank(Guid id)
        {
            if(Guid.Empty == id)
            {
                return BadRequest();
            }
            try
            {
                var questionBank = await _questionBankService.GetQuestionBankByIdAsync(id);

                if (questionBank == null)
                {
                    return NotFound(new { message = $"Question bank with ID {id} not found." });
                }

                var questions = await _questionBankService.GetQuestionsByQuestionBankAsync(id);

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = questions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/QuestionBanks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutQuestionBank(Guid id, QuestionBankDto dto)
        {
            if (Guid.Empty == id || !ModelState.IsValid || id != dto.Id)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedQuestionBank = await _questionBankService.UpdateQuestionBankAsync(id, dto);

                if (updatedQuestionBank == null)
                {
                    return NotFound(new { message = $"Question bank with ID {id} not found." });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = updatedQuestionBank
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/QuestionBanks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostQuestionBank(QuestionBankDto dto)
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
                var newQuestionBank = await _questionBankService.AddQuestionBankAsync(dto);

                if (newQuestionBank == null)
                {
                    return StatusCode(500, new { message = "Internal server error.", error = "Failed to add new question bank" });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = newQuestionBank
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        // DELETE: api/QuestionBanks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionBank(Guid id)
        {
            if(Guid.Empty == id)
            {
                return BadRequest();
            }

            try
            {
                var isDeleted = await _questionBankService.DeleteQuestionBankAsync(id);

                if(!isDeleted)
                {
                    return NotFound(new { message = $"Question bank with ID {id} not found." });
                }

                return Ok(new { message = "Deleted Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpPost("Add-List-Questions")]
        public async Task<ActionResult> AddListQuestionsToQuestionBank([FromBody]AddListQuestionToBankDto request )
        {
            if (request.Questions == null || !request.Questions.Any() || Guid.Empty == request.QuestionBankId)
            {
                return BadRequest(new { message = "Invalid input data." });
            }
            try
            {
                var addedQuestions = await _questionBankService.AddListQuestionsToQuestionBankAsync(request.QuestionBankId,request.Questions);
                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = addedQuestions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
