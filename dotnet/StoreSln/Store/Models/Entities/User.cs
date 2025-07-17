using System;
using System.Collections.Generic;

namespace Store.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
public class ReturnUser
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}

/// <summary>
/// Model to return upon successful login (user data + token)
/// </summary>
public class LoginResponse
{
    public ReturnUser User { get; set; }
    public string Token { get; set; }
}

/// <summary>
/// Model to accept login parameters
/// </summary>
public class LoginUser
{
    public string Username { get; set; }
    public string Password { get; set; }
}

/// <summary>
/// Model to accept registration parameters
/// </summary>
public class RegisterUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Role { get; set; }
}
