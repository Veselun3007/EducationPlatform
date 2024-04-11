using Identity.Core.DTO.Requests;
using Identity.Core.Services;
using Identity.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/userManagement")]
    public class UserController(UserOperation operation) : BaseController
    {
        private readonly UserOperation _operation = operation;

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDTO entity)
        {
            var id = User.FindFirst("username")!.Value;
            var result = await _operation.UpdateAsync(entity, id);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var id = User.FindFirst("username")!.Value;
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetUserByIdAsync()
        {
            var id = User.FindFirst("username")!.Value;
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }
    }
}
