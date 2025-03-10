using Identity.Core.DTO.Requests;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthController(UserService userOperation, IdentityService identityOperation) : Controller
    {
        private readonly UserService _userOperation = userOperation;
        private readonly IdentityService _identityOperation = identityOperation;

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromForm] UserDTO model)
        {
            var result = await _identityOperation.SignUpAsync(model.Email, model.Password);
            return Ok(result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ComfirmUserAsync([FromForm] ConfirmEmailRequest model)
        {
            await _identityOperation.ComfirmUserAsync(model.Email, model.ConfirmCode);
            return Ok();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromForm] LoginRequest model)
        {
            var result = await _identityOperation.SignInAsync(model.Email, model.Password);
            return Ok(result);
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] ConfirmResetPasswordRequest model)
        {
            await _identityOperation.SendPasswordResetEmail(model.Email);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest model)
        {
            await _identityOperation.ResetPassword(model.Email, model.ConfirmCode, model.Password);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestModel model)
        {
            var result = await _identityOperation.RefreshTokensAsync(model.RefreshToken, model.Email);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutAsync([FromBody] SignOutRequest model)
        {
            await _identityOperation.SignOutAsync(model.AccessToken);
            return Ok();
        }
    }
}
