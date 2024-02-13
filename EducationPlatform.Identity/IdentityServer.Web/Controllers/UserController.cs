using IdentityServer.Core.Interfaces;
using IdentityServer.Core.Services;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Controllers
{
    public class UserController(IBusinessUserOperation userOperation,
        UserService userService) : Controller
    {
        private readonly IBusinessUserOperation _userOperation = userOperation;
        private readonly UserService _userService = userService;

        [Route("updateUser/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, UserDTO entity)
        {
            var user = await _userOperation.UpdateAsync(entity, id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("deleteUser/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userOperation.DeleteAsync(id);
            return Ok();
        }

        [Route("getUserData/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserDada(int id)
        {
            var user = await _userOperation.GetUserAsync(id);
            if(user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
