using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentTeacherManagment.Core.DTOs;

namespace StudentTeacherManagment.Core.Services
{
    public class AuthService : IAuthService
    {
        private const string PasswordPattern = @"^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{6,})\S$";
        private const string EmailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$";
        private const int MinCodeValue = 100_000;
        private const int MaxCodeValue = 1_000_000;

        private readonly IRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        private static IDictionary<int, User> _unverifiedUsers = new Dictionary<int, User>();
        private static IDictionary<int, (User User, DateTime ExpiresAt)> _usersToResetPassword = new Dictionary<int, (User, DateTime)>();

        public AuthService(IRepository repository, IEmailSender emailSender, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _emailSender = emailSender;
        }

        private static TimeSpan MaxVerificationTime => TimeSpan.FromMinutes(10);

        public Task Register(User user)
        {
            // Validate user input
            ValidateUser(user);

            // Set the default role (e.g., "Student") or based on the incoming data
            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "Student"; // Default role
            }
            else if (user.Role != "Student" && user.Role != "Teacher")
            {
                throw new ArgumentException("Invalid role", nameof(user.Role));
            }

            // Generate a verification code
            var code = new Random().Next(MinCodeValue, MaxCodeValue);
            user.CreatedAt = DateTime.UtcNow;
            _unverifiedUsers.Add(code, user);

            // Send verification code to the user's email
            return _emailSender.Send("Your verification code: " + code);
        }
        
        private void ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new ArgumentException("First name is invalid", nameof(user.FirstName));
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new ArgumentException("Last name is invalid", nameof(user.LastName));
            }
            if (user.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth is invalid", nameof(user.DateOfBirth));
            }
            if (!Regex.IsMatch(user.Email, EmailPattern))
            {
                throw new ArgumentException("Email is invalid", nameof(user.Email));
            }
            if (!Regex.IsMatch(user.Password, PasswordPattern))
            {
                throw new ArgumentException("Password is invalid", nameof(user.Password));
            }
        }

        public async Task<UserDto?> Login(string email, string password)
        {
            var user = await _repository.GetAll<Student>()
                .FirstOrDefaultAsync(s => s.Email.Equals(email) && s.Password.Equals(password));

            if (user == null) return null; // Invalid credentials

            // Create JWT token with claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = tokenString;
            return userDto;
        }

        public async Task<User> ValidateAccount(string email, int code)
        {
            // Check code with email
            if (_unverifiedUsers.TryGetValue(code, out var unverifiedUser))
            {
                if (unverifiedUser.Email.Equals(email) && (DateTime.UtcNow - unverifiedUser.CreatedAt) < MaxVerificationTime)
                {
                    var student = new Student()
                    {
                        FirstName = unverifiedUser.FirstName,
                        LastName = unverifiedUser.LastName,
                        Email = unverifiedUser.Email,
                        Password = unverifiedUser.Password,
                        DateOfBirth = unverifiedUser.DateOfBirth,
                        CreatedAt = DateTime.UtcNow,
                    };

                    // Add the student to the database
                    await _repository.AddAsync(student);
                    await _repository.SaveChangesAsync();

                    return student;
                }
            }

            throw new ArgumentException("Code or email is incorrect");
        }

        public async Task<bool> SendPasswordResetCode(string email)
        {
            var user = await _repository.GetAll<Student>()
                .FirstOrDefaultAsync(s => s.Email.Equals(email));

            if (user == null)
                return false;

            var resetCode = new Random().Next(MinCodeValue, MaxCodeValue);
            _usersToResetPassword.Add(resetCode, (user, DateTime.UtcNow.AddMinutes(15)));

            await _emailSender.Send($"Your password reset code is: {resetCode}");

            return true;
        }

        public async Task<bool> ResetPassword(string email, int code, string newPassword)
        {
            if (!_usersToResetPassword.TryGetValue(code, out var resetEntry)) return false;
            var (user, expiresAt) = resetEntry;

            if (!user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) || DateTime.UtcNow > expiresAt) return false;

            if (!Regex.IsMatch(newPassword, PasswordPattern))
            {
                throw new ArgumentException("The new password does not meet the required criteria.");
            }

            user.Password = newPassword;

            if (user is not Student student)
                throw new InvalidCastException("The user could not be cast to a Student.");

            // Update the student's password in the database
            _repository.Update(student);
            await _repository.SaveChangesAsync();

            // Remove the reset code after successful reset
            _usersToResetPassword.Remove(code);

            return true;
        }
    }
}
