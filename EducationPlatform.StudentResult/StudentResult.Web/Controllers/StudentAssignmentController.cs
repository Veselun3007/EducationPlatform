using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentResult.Application.DTO.Request;
using StudentResult.Application.Services;

namespace StudentResult.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentAssignmentController : Controller
    {
        private readonly StudentAssignmentService _studentAssignmentService;

        public StudentAssignmentController(StudentAssignmentService studentAssignmentService)
        {
            _studentAssignmentService = studentAssignmentService;
        }

        // можна розділити на два метода, для вчителя (int studentAssignmentId) та студента (int assignmentId, string userId), но запарно
        // вертає модель (SA, список коментів + UserInfo, список (назва з aws та fileId))
        // добавити для файлів в базі назву -
        // (int assignmentId, string userId(з токена))
        
        [HttpGet("getStudentAssignment/{assignmentId}")]
        public async Task<IActionResult> GetStudentAssignment(int assignmentId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _studentAssignmentService.GetStudentAssignmentAsync(userId, assignmentId);
            return Ok(result);
        }

        // вертає список (SA, список коментів, список (назва з aws та fileId), UserInfo(UserName, UserImageLink, UserId) )
        [HttpGet("getAllStudentAssignment/{assignmentId}")]
        public async Task<IActionResult> GetAllStudentAssignments(int assignmentId)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _studentAssignmentService.GetAllStudentAssignmentAsync(userId, assignmentId);
            return Ok(result);
        }

        // вертає тимчасове посилання на файл з aws  
        // приймає int file_id
        [HttpGet("getFileLink/{fileId}")]
        public async Task<IActionResult> GetFileLink(int fileId)
        {
            var result = await _studentAssignmentService.GetFileLinkAsync(fileId);
            return Ok(result);
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
        [HttpPost("createComment")]
        public async Task<IActionResult> PostComment(CommentDto commentDto)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _studentAssignmentService.CreateCommentAsync(userId, commentDto);
            return Ok(result);
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
        [HttpPut("evaluation")]
        public async Task<IActionResult> UpdateMark(UpdateMarkDto updateMarkDto)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _studentAssignmentService.UpdateMarkAsync(userId, updateMarkDto);
            return Ok(result);
        }

        [HttpPut("updateWork")]
        public async Task<IActionResult> UpdateStudentAssignment([FromForm] UpdateStudentAssignmentDto studentAssignmentDto)
        {
            string? userId = HttpContext.User.FindFirst("username")?.Value;
            var result = await _studentAssignmentService.UpdateStudentAssignmentAsync(userId, studentAssignmentDto);
            return Ok(result);
        }
    }
}
