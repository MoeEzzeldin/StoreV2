using Microsoft.EntityFrameworkCore;
using Dapper;
using Store.Data;
using Store.Models;
using Store.Reposatories.I_Repos;
using Store.Utils;
using Store.Security;
using Store.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.Repos
{
    public class UserRepo : I_UserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IDapperContext _dapperContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserRepo(
            ApplicationDbContext context, 
            IDapperContext dapperContext,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        }

        #region IQueryable Methods (Entity Framework)
        
        public IQueryable<User> GetAllUsers()
        {
            return _context.Users.AsQueryable();
        }

        #endregion

        #region Async Methods (Mix of EF Core and Dapper)

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<User>("SELECT * FROM users");
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM users WHERE user_id = @Id", 
                new { Id = id });
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM users WHERE username = @Username", 
                new { Username = username });
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            using var connection = _dapperContext.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM users WHERE username = @Username", 
                new { Username = username });
            
            return count > 0;
        }

        public async Task<ReturnUser> AuthenticateUserAsync(LoginUser loginUser)
        {
            var user = await GetUserByUsernameAsync(loginUser.Username);
            
            if (user == null)
                return null;
                
            // Verify password
            bool isPasswordValid = _passwordHasher.VerifyHashMatch(
                user.PasswordHash, 
                loginUser.Password, 
                user.Salt
            );
            
            if (!isPasswordValid)
                return null;
                
            // Create return user
            var returnUser = new ReturnUser
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.UserRole
            };
            
            return returnUser;
        }

        public async Task<ReturnUser> RegisterUserAsync(RegisterUser registerUser)
        {
            // Check if username exists
            if (await UsernameExistsAsync(registerUser.Username))
                return null;
                
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
            
            // Add user to database
            user = await AddUserAsync(user);
            
            // Create return user
            var returnUser = new ReturnUser
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.UserRole
            };
            
            return returnUser;
        }

        public async Task<User> AddUserAsync(User user)
        {
            using var connection = _dapperContext.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(
                @"INSERT INTO users (username, password_hash, salt, user_role) 
                  VALUES (@Username, @PasswordHash, @Salt, @UserRole);
                  SELECT CAST(SCOPE_IDENTITY() as int)",
                new { 
                    user.Username,
                    user.PasswordHash,
                    user.Salt,
                    user.UserRole
                });
            
            user.UserId = id;
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            using var connection = _dapperContext.CreateConnection();
            await connection.ExecuteAsync(
                @"UPDATE users SET 
                    username = @Username,
                    password_hash = @PasswordHash,
                    salt = @Salt,
                    user_role = @UserRole
                  WHERE user_id = @UserId",
                new { 
                    user.Username,
                    user.PasswordHash,
                    user.Salt,
                    user.UserRole,
                    user.UserId
                });
            
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // First delete associations in user_review table
            using var connection = _dapperContext.CreateConnection();
            await connection.ExecuteAsync(
                "DELETE FROM user_review WHERE user_id = @Id",
                new { Id = id });
                
            // Then delete the user
            var affectedRows = await connection.ExecuteAsync(
                "DELETE FROM users WHERE user_id = @Id",
                new { Id = id });
            
            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryAsync<User>(
                "SELECT * FROM users WHERE user_role = @Role",
                new { Role = role });
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            using var connection = _dapperContext.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "UPDATE users SET user_role = @Role WHERE user_id = @Id",
                new { Id = userId, Role = newRole });
            
            return affectedRows > 0;
        }

        #endregion
    }
}
