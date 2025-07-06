using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Data;
using Store.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webstore.Security;

namespace Store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(
            ILogger<AuthController> logger,
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = passwordHasher;
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

                // Check if username is already taken
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Username is already taken" });
                }

                // Hash the password
                var hashedPassword = _passwordHasher.ComputeHash(model.Password);

                // Create new user
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = hashedPassword.Password,
                    Salt = hashedPassword.Salt,
                    UserRole = string.IsNullOrEmpty(model.Role) ? "User" : model.Role
                };

                // Save to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully: {Username}", model.Username);

                // Return success without sensitive data
                return Ok(new { 
                    message = "Registration successful", 
                    user = new ReturnUser 
                    { 
                        UserId = user.UserId, 
                        Username = user.Username, 
                        Role = user.UserRole 
                    } 
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

                // Find user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Verify password
                bool isPasswordValid = _passwordHasher.VerifyHashMatch(
                    user.PasswordHash,
                    model.Password,
                    user.Salt);

                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Generate JWT token
                string token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.UserRole);

                // Return successful login response with token
                var response = new LoginResponse
                {
                    User = new ReturnUser
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Role = user.UserRole
                    },
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