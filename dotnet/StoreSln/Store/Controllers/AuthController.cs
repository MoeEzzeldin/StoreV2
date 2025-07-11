using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Models;
using System;
using System.Threading.Tasks;
using Store.Security;
using Store.Reposatories.I_Repos;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly I_UserRepo _userRepo;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(
            ILogger<AuthController> logger,
            I_UserRepo userRepo,
            ITokenGenerator tokenGenerator)
        {
            _logger = logger;
            _userRepo = userRepo;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            try
            {
                // Validate request
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest(new { message = "Passwords do not match" });
                }

                // Check if username already exists
                if (await _userRepo.UsernameExistsAsync(model.Username))
                {
                    return BadRequest(new { message = "Username is already taken" });
                }

                // Register the user using the repository
                var user = await _userRepo.RegisterUserAsync(model);

                if (user == null)
                {
                    return BadRequest(new { message = "Registration failed" });
                }

                _logger.LogInformation("User registered successfully: {Username}", model.Username);

                // Return success without sensitive data
                return Ok(new { 
                    message = "Registration successful", 
                    user = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser model)
        {
            try
            {
                // Validate request
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                // Authenticate the user using the repository
                var user = await _userRepo.AuthenticateUserAsync(model);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Generate JWT token
                string token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.Role);

                // Return successful login response with token
                var response = new LoginResponse
                {
                    User = user,
                    Token = token
                };

                _logger.LogInformation("User logged in successfully: {Username}", model.Username);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }
    }
}