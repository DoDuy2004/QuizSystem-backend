using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(Roles = "TEACHER")]

    public class CourseClassesController : ControllerBase
    {
        private readonly ICourseClassService _courseClassService;

        public CourseClassesController(ICourseClassService courseClassService)
        {
            _courseClassService = courseClassService;
        }

        // GET: api/CourseClasses
        [HttpGet]
        public async Task<ActionResult> GetCourseClasses()
        {
            try
            {
                var result = await _courseClassService.GetCourseClassesAsync();

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

        // GET: api/CourseClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourseClass(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var courseClass = await _courseClassService.GetCourseClassByIdAsync(id);

                if (courseClass == null)
                {
                    return NotFound(new { message = $"Course class with ID {id} not found." });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = courseClass
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("subjects")]
        public async Task<ActionResult> GetSubjects()
        {
            try
            {
                var subjects = await _courseClassService.GetSubjectsAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = subjects
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/CourseClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseClass(Guid id, CourseClassDto dto)
        {
            if (id != dto.Id || !ModelState.IsValid || id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var updatedCourseClass = await _courseClassService.UpdateCourseClassAsync(id, dto);

                if(updatedCourseClass == null)
                {
                    return NotFound(new {message = $"Course class with {id} not found"});
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = updatedCourseClass
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        //POST: api/CourseClasses
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseClass>> PostCourseClass(CourseClassDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var newCourseClass = await _courseClassService.AddCourseClassAsync(dto);

                if(newCourseClass == null)
                {
                    return StatusCode(500, new { message = "Internal server error.", error = "Failed to add new course class" });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = newCourseClass
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        //DELETE: api/CourseClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseClass(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var isDeleted = await _courseClassService.DeleteCourseClassAsync(id);

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

        [HttpPost("{id}/add-student")]
        public async Task<ActionResult> AddStudentToCourse(StudentCourseClassDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var (success, message, data) = await _courseClassService.AddStudentToCourseAsync(dto);

                if (data == null)
                {
                    return NotFound(new { message = message });
                }

                return Ok(new {
                    code = 200,
                    message = "Success",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        [HttpGet("{id}/students")]
        public async Task<ActionResult> GetStudentsByClass(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var courseClass = await _courseClassService.GetCourseClassByIdAsync(id);

                if (courseClass == null)
                {
                    return NotFound(new { message = $"Course class with {id} not found" });
                }

                var students = await _courseClassService.GetStudentByCourseClassAsync(id);

                return Ok(new
                {
                    code = 200,
                    message = "Success",
                    data = students
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
