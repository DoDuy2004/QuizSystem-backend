using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.services;

namespace QuizSystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudenExamController : ControllerBase
    {
        private readonly IStudentExamService _studenExamService;

        public StudenExamController(IStudentExamService studentExamService)
        {
            _studenExamService = studentExamService;
        }

        
        
    }
}
