using CourseService.Application.Abstractions;
using CourseService.Application.Courses.Commands.CreateCourse;
using CourseService.Application.Courses.Commands.DeleteCourse;
using CourseService.Application.Courses.Commands.UpdateCourse;
using CourseService.Application.Courses.Queries.GetAllCourses;
using CourseService.Application.Courseusers.Commands.CreateCourseuser;
using CourseService.Application.DTOs.Courses.Queries;
using CourseService.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerResult {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IMediator mediator, IUnitOfWork unitOfWork) {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        //вертає CourseInfo
        [Authorize]
        [HttpGet("get_course")]
        public async Task<IActionResult> GetCourse(GetCourseQuery request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //вертає List CourseInfo
        [Authorize]
        [HttpGet("get_all_course")]
        public async Task<IActionResult> GetAllCourses(GetAllCoursesQuery request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //[Authorize]
        [HttpPost("create_course")]
        public async Task<IActionResult> PostCourse(CreateCourseCommand course_command) {
            //string user_id = HttpContext.User.FindFirst("username")?.Value;
            string user_id = "24d83448-e0c1-7036-9254-932db352e7e4";
            var result_course = await _mediator.Send(course_command, new CancellationToken());
            if (!result_course.IsSuccess) return ReturnResult(result_course);
            //var courseuser_command = new CreateCourseuserCommand(result_course.Value.CourseLink, user_id, 1, true);
            var courseuser_command = new CreateCourseuserCommand(result_course.Value, user_id, 1, true);
            var result_courseuser = await _mediator.Send(courseuser_command, new CancellationToken());
            var result = new Result<(Object?, Object?)>((result_course.Value, result_courseuser.Value));
            _unitOfWork.SaveChanges();
            return ReturnResult(result);
        }

        [Authorize]
        [HttpPut("update_course")]
        public async Task<IActionResult> PutCourse(UpdateCourseCommand request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        [Authorize]
        [HttpDelete("delete_course")]
        public async Task<IActionResult> DeleteCourse(DeleteCourseCommand request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }
    }
}
