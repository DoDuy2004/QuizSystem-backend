using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using QuizSystem_backend.services;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamServices _examService;
        private readonly IExamRepository _examRepository;

        public ExamsController(IExamServices examServices,IExamRepository examRepository)
        {
            _examService = examServices;
            _examRepository=examRepository;
        }

        [HttpGet("{id}/questions")]
        public async Task<ActionResult> GetQuestionsByExam(Guid id)
        {
            try
            {
                var result = await _examService.GetAllQuestionOfExam(id);
                if (result == null)
                {
                    return NotFound(new { message = "No questions found for this exam" });
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

        [HttpGet]
        public async Task<ActionResult> GetExams(string searchText = null)
        {
            try
            {
                var result = await _examService.GetExamsAsync();

                // Filter ngay tại controller nếu cần
                if (!string.IsNullOrEmpty(searchText))
                {
                    result = result.Where(e =>
                        e.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (e.Subject?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false));
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

        [HttpGet("{id}")]

        public async Task<ActionResult> GetExamById(Guid id)
        {
            try
            {
                var result = await _examService.GetExamByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { message = "Exam not found" });
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

        [HttpPost]

        public async Task<ActionResult> CreateExam([FromBody] ExamDto examDto)
        {
            try
            {
                if (examDto == null)
                {
                    return BadRequest(new { message = "Invalid exam data" });
                }
                var result = await _examService.AddExamAsync(examDto);
                if (result == null)
                {
                    return BadRequest(new { message = "Failed to create exam" });
                }
                return Ok(new
                {
                    code = 201,
                    message = "Exam created successfully",
                    data = result
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("{examId}/AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto question, Guid examId)
        {
            if (question == null || examId == null!) return BadRequest(new { message = "Value null" });
            var newQuestion =  await _examRepository.AddQuestionToExamAsync(new Question(question), examId);
            return Ok(new
            {
                code = 200,
                message = "Add question successfully",
                data = newQuestion
            });
        }
        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateExam(Guid id, [FromBody] ExamDto examDto)
        {
            try
            {
                if (examDto == null || id != examDto.Id)
                {
                    return BadRequest(new { message = "Invalid exam data" });
                }
                var result = await _examService.UpdateExamAsync(id, examDto);
                if (result == null)
                {
                    return NotFound(new { message = "Exam not found" });
                }
                return Ok(new
                {
                    code = 200,
                    message = "Exam updated successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteExam(Guid id)
        {
            try
            {
                var result = await _examService.DeleteExamAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Exam not found" });
                }
                return Ok(new { message = "Exam deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("create-matrix")]
        public async Task<IActionResult> CreateExamByMatrix([FromBody] ExamMatrixRequest request)
        {
            var result = await _examService.CreateExamByMatrixAsync(request);
            if (!result.Success)
                return BadRequest(new { errors = result.ErrorMessages });
            return Ok(result.Exam);
            
        }

        [HttpPost("{examId}/add-list-question")]
        public async Task<IActionResult> AddListQuestionToExam(Guid examId, [FromBody] List<QuestionDto> dto)
        {
            try
            {
                var result = await _examService.AddListQuestionToExamAsync(dto,examId);
                return Ok(new
                {
                    code = 201,
                    message = "Add list question successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
            

        }

        [HttpDelete("{examId}/questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestionFromExam(Guid examId, Guid questionId)
        {
            if(Guid.Empty == examId || Guid.Empty == questionId)
            {
                return BadRequest();
            }
            try
            {
                var isDeleted = await _examService.DeleteQuestionFromExamAsync(examId, questionId);
                if (!isDeleted)
                {
                    return NotFound(new { message = $"Question or Exam not found." });
                }

                return Ok(new { message = "Deleted Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchExams(string key,int limit)
        {
           
                var result = await _examService.SearchExam(key,limit);
                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = result
                });
          
        }


    }
}
