using Identity.Core.DTO.Requests;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/userManagement")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDTO entity)
        {
            var id = User.FindFirst("username")!.Value;
            var result = await _userService.UpdateAsync(entity, id);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var id = User.FindFirst("username")!.Value;
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserByIdAsync()
        {
            var id = User.FindFirst("username")!.Value;
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
