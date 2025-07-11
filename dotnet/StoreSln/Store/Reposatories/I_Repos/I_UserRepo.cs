using Store.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Reposatories.I_Repos
{
    public interface I_UserRepo
    {
        // Synchronous operations with IQueryable for LINQ
        IQueryable<User> GetAllUsers();
        
        // Asynchronous operations
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        
        // Authentication operations
        Task<ReturnUser> AuthenticateUserAsync(LoginUser loginUser);
        Task<ReturnUser> RegisterUserAsync(RegisterUser registerUser);
        
        // CRUD operations
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        
        // Role operations
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<bool> UpdateUserRoleAsync(int userId, string newRole);
    }
}
