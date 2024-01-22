using IdentityServer.Core.Interfaces;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Controllers
{
    public class UserController(IBusinessUserOperation userOperation): Controller
    {
        private readonly IBusinessUserOperation _userOperation = userOperation;

        [Route("updateUser/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateMaterial(int id, UserDTO entity)
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
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            await _userOperation.DeleteAsync(id);
            return Ok();
        }
    }
}
