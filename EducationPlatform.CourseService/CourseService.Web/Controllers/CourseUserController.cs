using CourseService.Application.DTO.Request;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseUserController : Controller
    {
        private readonly CourseuserService _courseuserService;

        public CourseUserController(CourseuserService courseuserService)
        {
            _courseuserService = courseuserService;
        }

        // вертає List CourseUserInfo
        [Authorize]
        [HttpGet("getCourseusersCourse")]
        public async Task<IActionResult> GetByIdCourse(int courseId)
        {
            var result = await _courseuserService.GetAllByCourseAsync(courseId);
            return Ok(result);
        }

        // нічого не повертати, крім статус кода
        [Authorize]
        [HttpPost("createCourseuser")]
        public async Task<IActionResult> PostStudent(StudentDTO request)
        {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            await _courseuserService.CreateStudentAsync(request);
            return Ok();
        }

        //повинно вертати CourseUserInfo
        [Authorize]
        [HttpPut("updateCourseuser")]
        public async Task<IActionResult> Put(UpdateCourseuserDTO request)
        {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _courseuserService.UpdateCourseuserAsync(request);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("deleteCourseuser/{courseuserId}")]
        public async Task<IActionResult> Delete(int courseuserId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            await _courseuserService.DeleteCourseuserAsync(userId, courseuserId);
            return Ok();
        }
    }
}
