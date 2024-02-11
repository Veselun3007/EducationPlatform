using IdentityServer.Core.DTOs.Token;
using IdentityServer.Core.Helpers;
using IdentityServer.Core.Interfaces;
using IdentityServer.Core.Services;
using IdentityServer.Web.DTOs.Login;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Mvc;


namespace EducationPlatform.Identity.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(
        UserService userService,
        IdentityOperation identityOperation,
        IBusinessUserOperation userOperation) : Controller
    {
        private readonly UserService _userService = userService;
        private readonly IdentityOperation _identityOperation = identityOperation;
        private readonly IBusinessUserOperation _userOperation = userOperation;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _identityOperation.FindByEmailAsync(loginDTO.Email);

            if (user is null || !CryptographyHelper.VerifyPassword(user.Salt,
                loginDTO.UserPassword, user.Password!))
            {
                return BadRequest("Invalid email or password");
            }
            var tokens = _userService.FromLoginToResponse(user);
            var tokenForStore = UserService.FromLoginToToken(user, tokens);
            var updateResult = await _identityOperation.AddTokenAsync(tokenForStore);

            if (updateResult)
            {
                return Ok(tokens);
            }

            return StatusCode(500, "Login error");
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] UserDTO userDTO)
        {
            var user = await _identityOperation.CreateAsync(userDTO);
            if (user is not null)
            {
                await _userOperation.AddAsync(userDTO);
                var userToken = await _identityOperation.AddTokenAsync(user.Id);
                var tokens = _userService.FromUserDtoToResponse(userDTO, userToken.RefreshToken);
                return Ok(tokens);
            }
            return StatusCode(500);
        }


        [HttpPost("getAccessToken")]
        public async Task<ActionResult> GetAccessToken([FromBody] GetAccessTokenDTO tokenDTO)
        {
            var existToken = await _identityOperation.FindTokenByParamAsync(tokenDTO.RefreshToken);

            if (existToken is null || existToken.RefreshTokenValidUntil <= DateTime.UtcNow)
            {
                await _identityOperation.DeleteTokenAsync(existToken.Id);
                return BadRequest("Invalid email or password");
            }

            var user = await _identityOperation.FindByIdAsync(existToken.UserId);
            var token = _userService.FromRequestToResponse(user);
            return Ok(token);
        }
    }
}

