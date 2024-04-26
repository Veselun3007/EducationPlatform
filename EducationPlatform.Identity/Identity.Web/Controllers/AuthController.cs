using Identity.Core.DTO.Requests;
using Identity.Core.DTO.User;
using Identity.Core.Services;
using Identity.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthController(UserService userOperation,
        IdentityService identityOperation) : BaseController
    {
        private readonly UserService _userOperation = userOperation;
        private readonly IdentityService _identityOperation = identityOperation;

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromForm] UserDTO model)
        {
            var result = await _identityOperation.SignUpAsync(model.Email, model.Password);
            if (result.IsSuccess)
            {
                var userResult = await _userOperation.AddAsync(model, result.Value);
                return FromResult(userResult);
            }
            return FromResult(result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ComfirmUserAsync([FromForm] ConfirmEmailRequest model)
        {
            var result = await _identityOperation.ComfirmUserAsync(model.Email, model.ConfirmCode);
            return FromResult(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromForm] LoginRequest model)
        {
            var result = await _identityOperation.SignInAsync(model.Email, model.Password);
            return FromResult(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestModel model)
        {
            var result = await _identityOperation.RefreshTokensAsync(model.RefreshToken);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutAsync([FromBody] SignOutRequest model)
        {
            var result = await _identityOperation.SignOutAsync(model.AccessToken);
            return FromResult(result);
        }
    }
}
