using Identity.Core.DTO.Auth;
using Identity.Core.DTO.Token;
using Identity.Core.DTO.User;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthController(
        UserOperation userOperation,
        IdentityOperation identityOperation) : Controller
    {
        private readonly UserOperation _userOperation = userOperation;
        private readonly IdentityOperation _identityOperation = identityOperation;

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromForm] UserDTO model)
        {
            var result = await _identityOperation.SignUpAsync(model.Email, model.Password);
            if (result.IsSuccess)
            {
                var userResult = await _userOperation.AddAsync(model, result.Value);
                if (userResult.IsSuccess)
                {
                    return Ok(userResult.Value);
                }
                return BadRequest(userResult.Error);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ComfirmUserAsync([FromForm] ConfirmEmailRequest model)
        {
            var result = await _identityOperation.ComfirmUserAsync(model.Email, model.ConfirmCode);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromForm] LoginModel model)
        {
            var result = await _identityOperation.SignInAsync(model.Email, model.Password);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestModel model)
        {
            var result = await _identityOperation.RefreshTokensAsync(model.RefreshToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutAsync([FromBody] SignOutRequest model)
        {
            var result = await _identityOperation.SignOutAsync(model.AccessToken);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.Error);
        }
    }
}
