using IdentityServer.Core.DTOs.Token;
using IdentityServer.Core.Helpers;
using IdentityServer.Core.Interfaces;
using IdentityServer.Core.Services;
using IdentityServer.Domain.Entities;
using IdentityServer.Web.DTOs.Login;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EducationPlatform.Identity.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(
        UserManager<AppUser> userManager,
        UserService userService,
        IConfiguration configuration,      
        IBusinessUserOperation userOperation) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly UserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IBusinessUserOperation _userOperation = userOperation;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null || !CryptographyHelper.VerifyPassword(user.Salt, 
                loginDTO.UserPassword, user.PasswordHash!))
            {
                return BadRequest("Invalid email or password");
            }

            var tokens = _userService.FromLoginToResponse(user);

            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenValidUntil = DateTime.UtcNow
                .AddDays(double.Parse(_configuration["JWT:RefreshTokenTtlInDays"]!));

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok(tokens);
            }

            return StatusCode(500, "Login error");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] UserDTO userDTO)
        {           
            var user = UserService.FromUserDtoToAppUser(userDTO);
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userOperation.AddAsync(userDTO);
                var tokens = _userService.FromUserDtoToResponse(userDTO, user.RefreshToken);
                return Ok(tokens);
            }
            return StatusCode(500); 
        }


        [HttpPost("getAccessToken")]
        public async Task<ActionResult> GetAccessToken([FromBody] GetAccessTokenDTO tokenDTO)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == tokenDTO.RefreshToken);

            if (user is null || user.RefreshTokenValidUntil <= DateTime.UtcNow)
            {
                return BadRequest("Invalid email or password");
            }

            var token = _userService.FromRequestToResponse(user);
            return Ok(token);
        }
    }
}

