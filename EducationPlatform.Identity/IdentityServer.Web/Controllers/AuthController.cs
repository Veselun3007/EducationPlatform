using Duende.IdentityServer.Services;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace EducationPlatform.Identity.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IIdentityServerInteractionService interactionService,
        EducationPlatformContext educationPlatformContext) : Controller
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IIdentityServerInteractionService _interactionService = interactionService;
        private readonly EducationPlatformContext _educationPlatformContext = educationPlatformContext;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            _ = await _userManager.CheckPasswordAsync(user, loginModel.UserPassword);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, 
                loginModel.UserPassword, false, false);
            
            if (result.Succeeded)
            {
                return Ok(_signInManager.ClaimsFactory);
            }

            ModelState.AddModelError(string.Empty, "Login error");
            return BadRequest(ModelState);
        }



        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] User userModel)
        {
            var user = new AppUser
            {
                UserName = userModel.UserName,
                Email = userModel.UserEmail
            };
            var result = await _userManager.CreateAsync(user, userModel.UserPassword);
            
            if (result.Succeeded)
            {
                var educationUser = new User
                {
                    UserName = userModel.UserName,
                    UserEmail = userModel.UserEmail,
                    UserPassword = userModel.UserPassword,
                    UserImage = userModel.UserImage
                };

                _educationPlatformContext.Users.Add(educationUser);
                await _educationPlatformContext.SaveChangesAsync();

                await _signInManager.SignInAsync(user, false);
                return Ok("succes");
            }

            return BadRequest(new { result.Errors });
        }



    }
}

