﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "TEACHER, ADMIN")]
    public class QuestionBanksController : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService;

        public QuestionBanksController(IQuestionBankService questionBankService)
        {
            _questionBankService = questionBankService;

        }

        // GET: api/QuestionBanks
        [HttpGet]
        public async Task<ActionResult> GetQuestionBanks(string searchText = null)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var questionBanks = await _questionBankService.GetQuestionBanksAsync(Guid.Parse(userId!));

                // Lọc trạng thái khác DELETED (nếu enum)
                questionBanks = questionBanks
                    .Where(q => q.Status != Status.DELETED)
                    .ToList();

                if (!string.IsNullOrEmpty(searchText))
                {
                    questionBanks = questionBanks
                        .Where(q =>
                            q.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            (q.Description?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

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
            if (id == Guid.Empty)
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

                var filteredQuestions = questions
                    .Where(q => q.Status != Enums.Status.DELETED) // Nếu là enum
                                                                    // .Where(q => q.Status != 2) // Nếu là int
                    .ToList();

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = filteredQuestions
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

        [HttpPost("{id}/Add-List-Questions")]
        public async Task<ActionResult> AddListQuestionsToQuestionBank(Guid id, [FromBody]List<QuestionImportPreviewDto> listPreview )
        {
            if(Guid.Empty == id)
            {
                return BadRequest();
            }

            if (listPreview == null || !listPreview.Any())
            {
                return BadRequest(new { message = "Danh sách không hợp lệ" });
            }
            try
            {
                var addedQuestions = await _questionBankService.ImPortQuestionConfirm(id, listPreview);
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

        [HttpPost("ImportQuestionsFile-preview")]
        public async Task<IActionResult> ImportQuestionsPreview(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ!");
            }
            try
            {
                var result = await _questionBankService.ImportQuestionsPreview(file);
                if (result == null || !result.Any())
                {
                    return NotFound(new { message = "No questions found in the file" });
                }
                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }
    }
}
