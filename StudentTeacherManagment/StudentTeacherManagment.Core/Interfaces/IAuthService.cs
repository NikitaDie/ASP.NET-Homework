using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Models;

namespace StudentTeacherManagment.Core.Interfaces;

public interface IAuthService
{
    Task Register(User user);
    Task<UserDto?> Login(string email, string password);

    Task<User> ValidateAccount(string email, int code);
    
    Task<bool> SendPasswordResetCode(string email);
    Task<bool> ResetPassword(string email, int code, string newPassword);

}