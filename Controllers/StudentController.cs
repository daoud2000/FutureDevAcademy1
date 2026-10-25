using Application.Dto.CoursesDto;
using Application.Dto.UserDto;
using Application.Repository.Interface;
using Application.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FutureDevAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) {

            _studentService = studentService;
        
        }
        [HttpPost("UserRegistration")]
        public async Task <IActionResult> UserRegistration([FromBody]UserRegistrationDto input) {

            await _studentService.UserRegistration(input);
            return Ok();
        
        }
        
        [Authorize( Roles ="Student")]
        [HttpGet("GetMyCourses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var response = await _studentService.GetMyCourses();
            return Ok(response);
        }
       
        [HttpGet("GetActiveCourses")]
        public async Task<IActionResult> GetActiveCourses([FromQuery] CourseFilterDto input)
        {
            var response = await _studentService.GetActiveCourses(input);
            return Ok(response);
        }
        [HttpPost("UpdateMyInfo")]
        public async Task<IActionResult> UpdateMyInfo([FromBody] UpdateUserDetails input)
        {
            await _studentService.UpdateMyInfo(input);
            return Ok();
        }

    }
}
