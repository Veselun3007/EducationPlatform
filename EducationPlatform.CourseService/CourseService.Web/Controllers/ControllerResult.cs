using CourseService.Application.Abstractions;
using CourseService.Application.Abstractions.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Web.Controllers {
    public class ControllerResult : ControllerBase {
        protected IActionResult ReturnResult<TValue>(Result<TValue> result) {
            return result.IsSuccess ? Success(result.Value) : Fail(result.Error);
        }
        protected IActionResult Success<TValue>(TValue value) {
            return Ok(value);
        }
        protected IActionResult Fail(Error error) {
            return StatusCode(error.HttpCode, error.Message);
        }
    }
}
