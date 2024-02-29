using CSharpFunctionalExtensions;
using Identity.Core.Models;
using Identity.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers.Base
{
    public class BaseController : Controller
    {

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(Error error)
        {
            var problem = Envelope.Error(error);
            return StatusCode(problem.StatusCode);
        }

        protected IActionResult FromResult<T>(Result<T, Error> result)
        {
            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}
