using CourseService.Application.DTO.Request;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly CoursesService _courseService;
        private readonly CourseuserService _courseuserService;
        public CourseController(CoursesService courseService, CourseuserService courseuserService)
        {
            _courseService = courseService;
            _courseuserService = courseuserService;
        }

        [HttpGet("getCourse")]
        public async Task<IActionResult> GetById(int courseId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.GetCourseAsync(userId, courseId);
            return Ok(result);
        }

        [HttpGet("getAllCourse")]
        public async Task<IActionResult> GetAll()
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.GetAllCourseAsync(userId);
            return Ok(result);
        }

        [HttpPost("createCourse")]
        public async Task<IActionResult> CreateCourse(CourseDTO course)
        {
            var resultCourse = await _courseService.CreateCourseAsync(course);
            resultCourse.UserId = HttpContext.User.FindFirst("username")?.Value;
            var resultCourseuser = await _courseuserService.CreateAdminAsync(resultCourse);
            return Ok(resultCourseuser);
        }

        [HttpPut("updateCourse")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDTO request)
        {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.UpdateCourseAsync(request);
            return Ok(result);
        }

        [HttpDelete("deleteCourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            await _courseService.DeleteCourseAsync(userId, courseId);
            return Ok();
        }
    }
}
