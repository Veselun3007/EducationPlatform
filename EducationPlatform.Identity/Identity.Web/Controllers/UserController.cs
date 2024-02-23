using Identity.Core.DTO.User;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/userManagement")]
    public class UserController(UserOperation operation) : Controller
    {
        private readonly UserOperation _operation = operation;

        [HttpPut]
        [Authorize]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, UserDTO entity)
        {
            var result = await _operation.UpdateAsync(entity, id);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var result = await _operation.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.Error);
        }
    }
}
