using CourseService.Application.DTO.Request;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers
{
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

        // вертає CourseInfo
        [Authorize]
        [HttpGet("get_course")]
        public async Task<IActionResult> GetById(int courseId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.GetCourseAsync(userId, courseId);
            return Ok(result);
        }

        // вертає List CourseInfo
        [Authorize]
        [HttpGet("get_all_course")]
        public async Task<IActionResult> GetAll()
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.GetAllCourseAsync(userId);
            return Ok(result);
        }

        // повинно вертати CourseInfo
        // але вертає NewCourseInfo
        [Authorize]
        [HttpPost("create_course")]
        public async Task<IActionResult> CreateCourse(CourseDTO course)
        {
            var result_course = await _courseService.CreateCourseAsync(course);
            result_course.UserId = HttpContext.User.FindFirst("username")?.Value;          
            var result_courseuser = await _courseuserService.CreateAdminAsync(result_course);
            return Ok(result_courseuser);
        }

        // повинно вертати CourseInfo
        [Authorize]
        [HttpPut("update_course")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDTO request)
        {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseService.UpdateCourseAsync(request);
            return Ok(result);
        }

        // нічого не вертає
        [Authorize]
        [HttpDelete("delete_course/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            await _courseService.DeleteCourseAsync(userId, courseId);
            return Ok();
        }
    }
}
