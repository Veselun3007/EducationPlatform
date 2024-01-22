using Amazon.Runtime.SharedInterfaces;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityServer.Core.DTOs.Login;
using IdentityServer.Core.Interfaces;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Web.DTOs.Login;
using IdentityServer.Web.DTOs.User;
using IdentityServer.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;


namespace EducationPlatform.Identity.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IIdentityServerInteractionService interactionService,
        EducationPlatformContext educationPlatformContext,
        TokenHelper tokenHelper,
        IConfiguration configuration,
        CryptographyHelper cryptographyHelper,
        IBusinessUserOperation userOperation) : Controller
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IIdentityServerInteractionService _interactionService = interactionService;
        private readonly EducationPlatformContext _educationPlatformContext = educationPlatformContext;
        private readonly TokenHelper _tokenHelper = tokenHelper;
        private readonly IConfiguration _configuration = configuration;
        private readonly CryptographyHelper _cryptographyHelper = cryptographyHelper;
        private readonly IBusinessUserOperation _userOperation = userOperation;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);



            if (user is null || !_cryptographyHelper.VerifyPassword(user.Salt, loginDTO.UserPassword, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return BadRequest(ModelState);
            }

            var tokens = new LoginResponseDTO()
            {
                AccessToken = _tokenHelper.GenerateAccessToken(user.UserName, user.Email),
                RefreshToken = _tokenHelper.GenerateRefreshToken()
            };

            user.RefreshToken = tokens.RefreshToken;
            user.ValidUntil = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenTtlInDays"]));

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {

                return Ok(tokens);
            }

            ModelState.AddModelError(string.Empty, "Login error");
            return StatusCode(500);
        }



        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] UserDTO userDTO)
        {
            var accessToken = _tokenHelper.GenerateAccessToken(userDTO.UserName, userDTO.UserEmail);
            var refreshToken = _tokenHelper.GenerateRefreshToken();
            var validUntil = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenTtlInDays"]));
            var salt = _cryptographyHelper.GenerateSalt();
            var hashedPassword = _cryptographyHelper.Hash(userDTO.UserPassword, salt);

            var user = new AppUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.UserEmail,
                PasswordHash = hashedPassword,
                Salt = salt,
                RefreshToken = refreshToken,
                ValidUntil = validUntil
            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userOperation.AddAsync(userDTO);

                var tokens = new LoginResponseDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                return Ok(tokens);
            }

            return StatusCode(500); ;
        }

        [HttpPost("getAccessToken")]
        public async Task<ActionResult> GetAccessToken([FromBody] GetAccessTokenDTO tokenDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == tokenDTO.RefreshToken);

            if (user is null || user.ValidUntil <= DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Invalid refresh token");
                return BadRequest(ModelState);
            }

            var token = new GetAccessTokenResponseDTO()
            {
                AccessToken = _tokenHelper.GenerateAccessToken(user.UserName, user.Email)
            };
            return Ok(token);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            return Ok();
        }
    }
}

