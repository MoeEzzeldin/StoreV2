using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data;
using Store.Models.Entities;
using Store.Reposatories.I_Repos;
using Store.Security;
using Store.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.Repos
{
    /// <summary>
    /// Repository for user-related operations using Entity Framework Core
    /// </summary>
    public class UserRepo : I_UserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<UserRepo> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepo"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="passwordHasher">The password hashing service</param>
        /// <param name="tokenGenerator">The token generation service</param>
        /// <param name="logger">The logger</param>
        public UserRepo(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            ILogger<UserRepo> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region IQueryable Methods (Entity Framework)
        
        /// <summary>
        /// Gets an IQueryable of all users
        /// </summary>
        /// <returns>IQueryable of all users</returns>
        public IQueryable<User> GetAllUsers()
        {
            return _context.Users.AsQueryable();
        }

        #endregion

        #region Async Methods (Entity Framework)

        /// <summary>
        /// Gets all users asynchronously
        /// </summary>
        /// <returns>A collection of all users</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Getting all users");
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Gets a user by ID asynchronously
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>The user or null if not found</returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting user by ID: {UserId}", id);
                var user = await _context.Users.FindAsync(id);
                
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", id);
                }
                else
                {
                    _logger.LogInformation("User found with ID: {UserId}", id);
                }
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// Gets a user by username asynchronously
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The user or null if not found</returns>
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("GetUserByUsernameAsync called with null or empty username");
                return null;
            }
            
            try
            {
                _logger.LogInformation("Getting user by username: {Username}", username);
                // Use case-insensitive comparison for username
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                
                if (user == null)
                {
                    _logger.LogWarning("User not found with username: {Username}", username);
                }
                else
                {
                    _logger.LogInformation("User found: Username={Username}, UserId={UserId}, HasPassword={HasPassword}, HasSalt={HasSalt}",
                        user.Username,
                        user.UserId,
                        !string.IsNullOrEmpty(user.PasswordHash),
                        !string.IsNullOrEmpty(user.Salt));
                }
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by username: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Checks if a username exists asynchronously
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns>True if the username exists, false otherwise</returns>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            
            try
            {
                _logger.LogInformation("Checking if username exists: {Username}", username);
                return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if username exists: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Authenticates a user asynchronously
        /// </summary>
        /// <param name="loginUser">The login credentials</param>
        /// <returns>User information if authentication succeeds, null otherwise</returns>
        public async Task<ReturnUser> AuthenticateUserAsync(LoginUser loginUser)
        {
            try
            {
                _logger.LogInformation("Authenticating user: {Username}", loginUser?.Username);
                
                if (loginUser == null || string.IsNullOrWhiteSpace(loginUser.Username))
                {
                    _logger.LogWarning("Authentication failed: Login data is null or invalid");
                    return null;
                }
                
                var user = await GetUserByUsernameAsync(loginUser.Username);
                
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: User not found - {Username}", loginUser.Username);
                    return null;
                }
                
                _logger.LogDebug("Verifying password for user: {Username}", loginUser.Username);
                
                // Verify password
                bool isPasswordValid = _passwordHasher.VerifyHashMatch(
                    user.PasswordHash,
                    loginUser.Password,
                    user.Salt
                );
                
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Authentication failed: Invalid password for user - {Username}", loginUser.Username);
                    return null;
                }
                
                _logger.LogInformation("Authentication successful for user: {Username}", loginUser.Username);
                
                // Create return user
                var returnUser = new ReturnUser
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.UserRole
                };
                
                return returnUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for user: {Username}", loginUser?.Username);
                throw;
            }
        }

        /// <summary>
        /// Registers a new user asynchronously
        /// </summary>
        /// <param name="registerUser">The registration data</param>
        /// <returns>User information if registration succeeds, null otherwise</returns>
        public async Task<ReturnUser> RegisterUserAsync(RegisterUser registerUser)
        {
            try
            {
                _logger.LogInformation("Registering new user: {Username}", registerUser?.Username);
                
                if (registerUser == null || string.IsNullOrWhiteSpace(registerUser.Username))
                {
                    _logger.LogWarning("Registration failed: Register data is null or invalid");
                    return null;
                }
                
                // Check if username exists
                if (await UsernameExistsAsync(registerUser.Username))
                {
                    _logger.LogWarning("Registration failed: Username already exists - {Username}", registerUser.Username);
                    return null;
                }
                
                _logger.LogDebug("Computing password hash for new user");
                
                // Hash password
                PasswordHash passwordHash = _passwordHasher.ComputeHash(registerUser.Password);
                
                // Create new user
                var user = new User
                {
                    Username = registerUser.Username,
                    PasswordHash = passwordHash.Password,
                    Salt = passwordHash.Salt,
                    UserRole = registerUser.Role ?? "User" // Default role if not provided
                };
                
                _logger.LogDebug("Adding new user to database: {Username}", user.Username);
                
                // Add user to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("User registered successfully: {Username} with ID: {UserId}", user.Username, user.UserId);
                
                // Create return user
                var returnUser = new ReturnUser
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.UserRole
                };
                
                return returnUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for: {Username}", registerUser?.Username);
                throw;
            }
        }

        /// <summary>
        /// Adds a user asynchronously
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <returns>The added user</returns>
        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Adding user: {Username}", user?.Username);
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("User added successfully: {Username} with ID: {UserId}", user.Username, user.UserId);
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user: {Username}", user?.Username);
                throw;
            }
        }

        /// <summary>
        /// Updates a user asynchronously
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns>The updated user</returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Updating user: {Username} with ID: {UserId}", user?.Username, user?.UserId);
                
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("User updated successfully: {Username} with ID: {UserId}", user.Username, user.UserId);
                
                return user;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating user: {UserId}", user?.UserId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", user?.UserId);
                throw;
            }
        }

        /// <summary>
        /// Deletes a user asynchronously
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>True if deletion succeeded, false otherwise</returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", id);
                
                // Find the user
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Delete failed: User not found with ID: {UserId}", id);
                    return false;
                }
                
                // Handle associations through EF Core relationships
                // First load related reviews to ensure proper relationship tracking
                await _context.Entry(user)
                    .Collection(u => u.Reviews)
                    .LoadAsync();
                
                // Remove the user
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("User deleted successfully: ID: {UserId}", id);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// Gets users by role asynchronously
        /// </summary>
        /// <param name="role">The role to filter by</param>
        /// <returns>A collection of users with the specified role</returns>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            try
            {
                _logger.LogInformation("Getting users by role: {Role}", role);
                
                return await _context.Users
                    .Where(u => u.UserRole.ToLower() == role.ToLower())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by role: {Role}", role);
                throw;
            }
        }

        /// <summary>
        /// Updates a user's role asynchronously
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="newRole">The new role</param>
        /// <returns>True if update succeeded, false otherwise</returns>
        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            try
            {
                _logger.LogInformation("Updating role for user ID: {UserId} to {Role}", userId, newRole);
                
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Update role failed: User not found with ID: {UserId}", userId);
                    return false;
                }
                
                user.UserRole = newRole;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Role updated successfully for user ID: {UserId} to {Role}", userId, newRole);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role for user: {UserId}", userId);
                throw;
            }
        }

        #endregion
    }
}
