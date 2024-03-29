using Identity.Core.DTO.User;
using Identity.Core.Services;
using Identity.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/userManagement")]
    public class UserController(UserOperation operation) : BaseController
    {
        private readonly UserOperation _operation = operation;
      
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, UserDTO entity)
        {
            var result = await _operation.UpdateAsync(entity, id);
            return FromResult(result);
        }
       
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }
    }
}
