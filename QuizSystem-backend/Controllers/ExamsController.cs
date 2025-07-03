using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.services;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamServices _examService;

        public ExamsController(IExamServices examServices)
        {
            _examService = examServices;
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

        public async Task<ActionResult> GetExams()
        {
            try
            {
                var result = await _examService.GetExamsAsync();
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

        [HttpPost("{id}/add-list-question")]
        public async Task<IActionResult> AddListQuestionToExam(Guid id, [FromBody] AddListQuestionDto dto)
        {
            try
            {
                var result = await _examService.AddListQuestionToExamAsync(dto);
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
    }
}
