using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using HelthCareProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<user> _userManager;
        private readonly SignInManager<user> _signInManager;
        private readonly RoleManager<role> _roleManager;

        public AuthenticationController(UserManager<user> userManager,
                                         SignInManager<user> signInManager,
                                         RoleManager<role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Register API
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new user { FirstName = model.First_Name, LastName = model.Last_Name, Email = model.Email, PhoneNumber = model.PhoneNumber };    
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign Role (if provided)
            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new role { Name = model.Role });
                }
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { message = "User registered successfully!" });
        }

        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid Email or Password.");
            }

            return Ok(new { message = "Login successful!" });
        }
    }

    public class RegisterModel
    {
        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
