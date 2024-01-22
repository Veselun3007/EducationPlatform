﻿using Duende.IdentityServer.Services;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Web.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginModel.UserPassword))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.UserPassword, false, false);

            if (result.Succeeded)
            {
                
                return Ok();
            }

            ModelState.AddModelError(string.Empty, "Login error");
            return BadRequest(ModelState);
        }




        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserDTO userModel)
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
                    UserEmail = userModel.UserEmail
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

