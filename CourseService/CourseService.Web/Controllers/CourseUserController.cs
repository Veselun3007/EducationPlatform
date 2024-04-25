using CourseService.Application.Courseusers.Commands.CreateCourseuser.CreateStudent;
using CourseService.Application.Courseusers.Commands.DeleteCourseuser;
using CourseService.Application.Courseusers.Commands.UpdateCourseuser;
using CourseService.Application.Courseusers.Queries.GetCourseusersByCourse;
using CourseService.Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CourseUserController : ControllerResult {
        private readonly IMediator _mediator;

        public CourseUserController(IMediator mediator, EducationPlatformContext context) {
            _mediator = mediator;
        }

        // вертає List CourseUserInfo
        //[Authorize]
        [HttpGet("get_courseusers_course")]
        public async Task<IActionResult> GetByIdCourse(int courseId) {
            GetCourseusersByCourseQuery request = new GetCourseusersByCourseQuery(courseId);
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //[Authorize]
        [HttpPost("create_courseuser")]
        public async Task<IActionResult> PostCourseuser(CreateStudentCommand request) {
            request.IsAdmin = false;
            request.Role = 2;
            //request.UserId = HttpContext.User.FindFirst("username")?.Value;
            request.UserId = "945864e8-30e1-7010-6377-79d39e0c3261";
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //[Authorize]
        [HttpPut("update_courseuser")]
        public async Task<IActionResult> Put(UpdateCourseuserCommand request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //[Authorize]
        [HttpDelete("delete_courseuser")]
        public async Task<IActionResult> Delete(DeleteCourseuserCommand request) {
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }
    }
}
