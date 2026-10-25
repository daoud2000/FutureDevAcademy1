using Application.Dto.CoursesDto;
using Application.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FutureDevAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
       private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) {
        
        _courseService = courseService;
        
        
        
        }
        
       
        [HttpPost("CreateCourse")]
       
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto input)
        {
            await _courseService.CreateCourse(input);
            return Ok();
        }

        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteCourse(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("EnrollCourse")]
       
        public async Task<IActionResult> EnrollCourse([FromBody] EnrollCourseDto input)
        {
            await _courseService.EnrollCourse(input);
            return Ok();
        }
        
        [HttpGet("GetCourses")]
        
        public async Task<IActionResult> GetCourses([FromQuery]CourseFilterDto input)
        {
            var response = await _courseService.GetCourses(input);
            return Ok(response);
        }
        [HttpGet("GetTotalEnrollments")]
        public async Task<IActionResult> GetTotalEnrollments()
        {
            var total = await _courseService.GetTotalEnrollments();
            return Ok(total);
        }
        [HttpGet("GetCourseEnrollment")]

        public async Task<IActionResult> GetCourseEnrollment(int id)
        {
            var response = await _courseService.GetCourseEnrollment(id);
            return Ok(response);
        }

        [HttpPut("UpdateCours/{id}")]

        public async Task<IActionResult> UpdateCourse(int id , [FromBody]UpdateCourseDto input)
        {
            try
            {
                await _courseService.UpdateCourse(id, input);
                return Ok();


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            
            
        }
    }
}
