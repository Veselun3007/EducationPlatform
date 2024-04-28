using CourseService.Application.Abstractions;
using CourseService.Application.Courses.Commands.CreateCourse;
using CourseService.Application.Courses.Commands.DeleteCourse;
using CourseService.Application.Courses.Commands.UpdateCourse;
using CourseService.Application.Courses.Queries.GetAllCourses;
using CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateTeacher;
using CourseService.Application.DTOs;
using CourseService.Application.DTOs.Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerResult {
        private readonly IMediator _mediator;
        public CourseController(IMediator mediator) {
            _mediator = mediator;
        }

        // вертає CourseInfo
        [Authorize]
        [HttpGet("get_course")]
        public async Task<IActionResult> Get(int courseId) {
            GetCourseQuery request = new GetCourseQuery(courseId, HttpContext.User.FindFirst("username")?.Value);
            //GetCourseQuery request = new GetCourseQuery(courseId, "24d83448-e0c1-7036-9254-932db352e7e4");
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // вертає List CourseInfo
        [Authorize]
        [HttpGet("get_all_course")]
        public async Task<IActionResult> GetAll() {
            GetAllCoursesQuery request = new GetAllCoursesQuery(HttpContext.User.FindFirst("username")?.Value);
            //GetAllCoursesQuery request = new GetAllCoursesQuery("24d83448-e0c1-7036-9254-932db352e7e4");
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // повинно вертати CourseInfo
        // але вертає NewCourseInfo
        [Authorize]
        [HttpPost("create_course")]
        public async Task<IActionResult> Post(CreateCourseCommand course_command) {
            string user_id = HttpContext.User.FindFirst("username")?.Value;
            //string user_id = "24d83448-e0c1-7036-9254-932db352e7e4";

            var result_course = await _mediator.Send(course_command, new CancellationToken());
            if (!result_course.IsSuccess) return ReturnResult(result_course);

            var courseuser_command = new CreateAdminCommand(new NewCourseInfo(result_course.Value), user_id);
            var result_courseuser = await _mediator.Send(courseuser_command, new CancellationToken());

            if (!result_courseuser.IsSuccess) return ReturnResult(result_courseuser);

            return ReturnResult(result_courseuser);
        }

        // повинно вертати CourseInfo
        [Authorize]
        [HttpPut("update_course")]
        public async Task<IActionResult> Put(UpdateCourseCommand request) {
            //request.UserId = "24d83448-e0c1-7036-9254-932db352e7e4";
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // нічого не вертає
        [Authorize]
        [HttpDelete("delete_course")]
        public async Task<IActionResult> Delete(DeleteCourseCommand request) {
            //request.UserId = "24d83448-e0c1-7036-9254-932db352e7e4";
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }
    }
}
