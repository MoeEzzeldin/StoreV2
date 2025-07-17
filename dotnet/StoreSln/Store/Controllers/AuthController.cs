using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Models.Entities;
using Store.Reposatories.I_Repos;
using Store.Security;
using System.Net;
using System.Threading.Tasks;
using Store.Models.DTOs;
using AutoMapper;
namespace Store.Controllers
{
    /// <summary>
    /// Controller responsible for authentication operations including user registration and login
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly I_UserRepo _userRepo;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class
        /// </summary>
        /// <param name="logger">Logger for recording authentication events</param>
        /// <param name="userRepo">Repository for user data operations</param>
        /// <param name="tokenGenerator">Service for generating authentication tokens</param>
        public AuthController(
            ILogger<AuthController> logger,
            I_UserRepo userRepo,
            ITokenGenerator tokenGenerator,
            IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;

            _logger.LogDebug("AuthController initialized");
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>JWT token and user information</returns>
        /// <response code="200">Returns JWT token and user information</response>
        /// <response code="400">If the model is invalid</response>
        /// <response code="401">If authentication fails</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUser model)
        {
            _logger.LogInformation("Login attempt received for user: {Username}", model?.Username ?? "null");
            
            try
            {
                // Validate model
                if (model == null)
                {
                    _logger.LogWarning("Login failed: Request body is null");
                    return BadRequest(new ErrorResponse("Request body is required"));
                }
                
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    _logger.LogWarning("Login failed: Username or password is empty");
                    return BadRequest(new ErrorResponse("Username and password are required"));
                }

                // Authenticate user
                _logger.LogDebug("Attempting to authenticate user: {Username}", model.Username);
                ReturnUser user = await _userRepo.AuthenticateUserAsync(model);

                if (user == null)
                {
                    _logger.LogWarning("Authentication failed for username: {Username}", model.Username);
                    return Unauthorized(new ErrorResponse("Invalid username or password"));
                }

                // Generate JWT token
                _logger.LogDebug("Generating token for user: {Username} with role: {Role}", user.Username, user.Role);
                string token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.Role);

                // Create and return response
                LoginResponse response = new LoginResponse
                {
                    User = user,
                    Token = token
                };

                _logger.LogInformation("User logged in successfully: {Username} with role: {Role}", user.Username, user.Role);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt for user: {Username}", model?.Username ?? "unknown");
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new ErrorResponse("An error occurred during login. Please try again later."));
            }
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="model">Registration details</param>
        /// <returns>Newly created user information</returns>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">If the model is invalid or username already exists</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(SuccessResponse<ReturnUser>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            _logger.LogInformation("Registration request received for username: {Username}", model?.Username ?? "null");
            
            try
            {
                // Validate model
                if (model == null)
                {
                    _logger.LogWarning("Registration failed: Request body is null");
                    return BadRequest(new ErrorResponse("Request body is required"));
                }
                
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    _logger.LogWarning("Registration failed: Username or password is empty");
                    return BadRequest(new ErrorResponse("Username and password are required"));
                }

                if (model.Password != model.ConfirmPassword)
                {
                    _logger.LogWarning("Registration failed: Passwords do not match for username: {Username}", model.Username);
                    return BadRequest(new ErrorResponse("Passwords do not match"));
                }

                // Check for existing username
                _logger.LogDebug("Checking if username already exists: {Username}", model.Username);
                if (await _userRepo.UsernameExistsAsync(model.Username))
                {
                    _logger.LogWarning("Registration failed: Username already exists: {Username}", model.Username);
                    return BadRequest(new ErrorResponse("Username is already taken"));
                }

                // Register the new user
                _logger.LogDebug("Registering new user: {Username} with role: {Role}", 
                    model.Username, !string.IsNullOrWhiteSpace(model.Role) ? model.Role : "User (default)");
                var user = await _userRepo.RegisterUserAsync(model);

                if (user == null)
                {
                    _logger.LogError("Registration failed: User creation returned null for username: {Username}", model.Username);
                    return BadRequest(new ErrorResponse("Registration failed due to an internal error"));
                }

                _logger.LogInformation("User registered successfully: {Username} with ID: {UserId} and role: {Role}", 
                    user.Username, user.UserId, user.Role);

                // Return success response
                return Ok(new SuccessResponse<ReturnUser>("Registration successful", user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", model?.Username ?? "unknown");
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new ErrorResponse("An error occurred during registration. Please try again later."));
            }
        }

        /// <summary>
        /// Checks if a specific username is available
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <returns>Availability status</returns>
        [HttpGet("check-username/{username}")]
        [ProducesResponseType(typeof(UsernameAvailabilityResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUsernameAvailability(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return Ok(new UsernameAvailabilityResponse { IsAvailable = false, Message = "Username cannot be empty" });
                }

                bool exists = await _userRepo.UsernameExistsAsync(username);
                return Ok(new UsernameAvailabilityResponse 
                { 
                    IsAvailable = !exists,
                    Message = exists ? "Username is already taken" : "Username is available"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username availability: {Username}", username);
                return Ok(new UsernameAvailabilityResponse
                { 
                    IsAvailable = false,
                    Message = "Error checking username availability"
                });
            }
        }
    }

    /// <summary>
    /// Standard error response format
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new error response with the specified message
        /// </summary>
        public ErrorResponse(string message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// Standard success response format with data
    /// </summary>
    public class SuccessResponse<T>
    {
        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Response data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Creates a new success response with the specified message and data
        /// </summary>
        public SuccessResponse(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }

    /// <summary>
    /// Response for username availability check
    /// </summary>
    public class UsernameAvailabilityResponse
    {
        /// <summary>
        /// Indicates if the username is available
        /// </summary>
        public bool IsAvailable { get; set; }
        
        /// <summary>
        /// Message describing the result
        /// </summary>
        public string Message { get; set; }
    }
}