using Identity.Core.DTO.Requests;
using Identity.Core.Interfaces;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly IIdentityService _identityService;

        public AuthController(UserService userService, IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromForm] UserDTO model)
        {
            var userSub = await _identityService.SignUpAsync(model.Email, model.Password);
            if(userSub is not null)
            {
                await _userService.AddAsync(model, userSub);
            }
            return Ok();
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ComfirmUserAsync([FromForm] ConfirmEmailRequest model)
        {
            await _identityService.ComfirmUserAsync(model.Email, model.ConfirmCode);
            return Ok();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromForm] LoginRequest model)
        {
            var result = await _identityService.SignInAsync(model.Email, model.Password);
            return Ok(result);
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] ConfirmResetPasswordRequest model)
        {
            await _identityService.SendPasswordResetEmail(model.Email);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest model)
        {
            await _identityService.ResetPassword(model.Email, model.ConfirmCode, model.Password);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestModel model)
        {
            var result = await _identityService.RefreshTokensAsync(model.RefreshToken, model.Email);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutAsync([FromBody] SignOutRequest model)
        {
            await _identityService.SignOutAsync(model.AccessToken);
            return Ok();
        }
    }
}
