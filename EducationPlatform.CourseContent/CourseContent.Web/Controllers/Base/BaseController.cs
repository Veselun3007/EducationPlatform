using CSharpFunctionalExtensions;
using Identity.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(MessageWrapper.Ok(result));
        }

        protected IActionResult Error(Error error)
        {
            return StatusCode(MessageWrapper.Error(error).StatusCode);
        }

        protected IActionResult FromResult<T>(Result<T, Error> result)
        {
            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}
