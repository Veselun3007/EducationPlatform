using Microsoft.AspNetCore.Mvc;
using StudentResult.Application.Abstractions;
using StudentResult.Application.Abstractions.Errors;

namespace StudentResult.Web.Controllers {
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
