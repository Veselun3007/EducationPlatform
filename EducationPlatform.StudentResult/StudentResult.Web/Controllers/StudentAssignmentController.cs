using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentResult.Application.Commands.CreateComment;
using StudentResult.Application.Commands.UpdateMark;
using StudentResult.Application.Commands.UpdateStudentAssignment;
using StudentResult.Application.Queries.GetAllStudentAssignment;
using StudentResult.Application.Queries.GetFileLink;
using StudentResult.Application.Queries.GetStudentAssignment;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Interfaces;

namespace StudentResult.Web.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class StudentAssignmentController : ControllerResult {
        private readonly IMediator _mediator;
        public StudentAssignmentController(IMediator mediator, IUnitOfWork unitOfWork) {
            _mediator = mediator;
        }

        // можна розділити на два метода, для вчителя (int studentAssignmentId) та студента (int assignmentId, string userId), но запарно
        // вертає модель (SA, список коментів + UserInfo, список (назва з aws та fileId))
        // добавити для файлів в базі назву -
        // (int assignmentId, string userId(з токена))
        [Authorize]
        [HttpGet("get_sa/{assignmentId}")]
        public async Task<IActionResult> GetStudentAssignment(int assignmentId) {
            GetStudentAssignmentQuery request = new GetStudentAssignmentQuery(assignmentId, HttpContext.User.FindFirst("username")?.Value);
            //GetStudentAssignmentQuery request = new GetStudentAssignmentQuery(assignmentId, "2428f4f8-c071-708f-229b-4763a5e3f703");
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // вертає список (SA, список коментів, список (назва з aws та fileId), UserInfo(UserName, UserImageLink, UserId) )
        [Authorize]
        [HttpGet("get_all_sa/{assignmentId}")]
        public async Task<IActionResult> GetAllStudentAssignments(int assignmentId) {
            GetAllStudentAssignmentQuery request = new GetAllStudentAssignmentQuery(assignmentId, HttpContext.User.FindFirst("username")?.Value);
            //GetAllStudentAssignmentQuery request = new GetAllStudentAssignmentQuery(assignmentId, "945864e8-30e1-7010-6377-79d39e0c3261");
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // вертає тимчасове посилання на файл з aws  
        // приймає int file_id
        [Authorize]
        [HttpGet("get_file_link/{file_id}")]
        public async Task<IActionResult> GetFileLink(int file_id) {
            //GetFileLinkQuery request = new GetFileLinkQuery(file_id, "24d83448-e0c1-7036-9254-932db352e7e4");
            GetFileLinkQuery request = new GetFileLinkQuery(file_id, HttpContext.User.FindFirst("username")?.Value);
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        // приймає (список файлів (може бути пустий), int assignmentId, int studentId)
        // або приймає (список файлів (може бути пустий), int assignmentId, string userId (з токена)) .. цей варіант
        // current_mark зробити null в базі
        // вертаю як при get_sa
        //
        // створення курсу породжує са для всіх студентів, початкові значенян null
        //
        //не буде
        //[Authorize]
        //[HttpPost("create_sa")]
        //public async Task<IActionResult> PostStudentAssignment() {

        //    var result = await _mediator.Send(request, new CancellationToken());
        //    return ReturnResult(result);
        //}

        // приймає string comment_text, int studentassignment_id, string userId (з токена) // цей варіант
        // або приймає string comment_text, int studentassignment_id, string courseUserId // якщо без перевірок, то це
        //вертає список коментів для цього sa + UserInfo для кожного комента
        [Authorize]
        [HttpPost("create_comment")]
        public async Task<IActionResult> PostComment(CreateCommentCommand request) {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            //request.UserId = "2428f4f8-c071-708f-229b-4763a5e3f703";
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        //повернення роботи студентом або вчителем (приймає int studentassignment_id, string userId (з токена))
        //
        //виставлення оцінки (приймає int currentMark, int studentassignment_id, string userId (з токена))
        //оновлення роботи студентом (приймає список файлів, int studentassignment_id, string userId (з токена))
        //при оновлені роботи спочатку видаляються всі попередні файли, якщо до цього були не видалені
        //
        //при поверненні роботи файли можна видаляти або ні. якщо не видаляти то при отриманні sa вчитель не побачить файлі, студент побачить
        //
        //
        //
        //виставлення оцінки (приймає int currentMark, int studentassignment_id, string userId (з токена))
        //вертає get_all_as тільки не список
        //якщо викладач хоче оцінити без роботи (is_done = false), то is_done = true
        //
        //оновлення роботи студентом (приймає список файлів, int studentassignment_id, string userId (з токена))
        //оновлення файлів (видалення старих і створення нових) та дати здачі і встановлення is_done = true. це все робиться якщо не стоїть оцінка!!!
        //вертає з гет_са
        //
        [Authorize]
        [HttpPut("evaluation")]
        public async Task<IActionResult> UpdateMark(UpdateMarkCommand request) {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            //request.UserId = "24d83448-e0c1-7036-9254-932db352e7e4";
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }

        [Authorize]
        [HttpPut("update_work")]
        public async Task<IActionResult> UpdateStudentAssignment([FromForm] UpdateStudentAssignmentCommand request) {
            request.UserId = HttpContext.User.FindFirst("username")?.Value;
            //request.UserId = "2428f4f8-c071-708f-229b-4763a5e3f703";
            var result = await _mediator.Send(request, new CancellationToken());
            return ReturnResult(result);
        }
    }
}
