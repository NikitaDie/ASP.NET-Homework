using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;

namespace StudentTeacherManagment.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto user)
    {
        await _authService.Register(_mapper.Map<Student>(user));
        return Ok();
    }

    [HttpPost("verifyUser")]
    public async Task<ActionResult<User>> VerifyUser([FromBody] VerificationData verificationData)
    {
        var user = await _authService.ValidateAccount(verificationData.Email, verificationData.Code);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginDto user)
    {
        var loggedUser = await _authService.Login(user.Email, user.Password);

        if (loggedUser == null)
            return NotFound("Invalid credentials.");
        
        return Ok(new { Token = loggedUser.Token });
    }
   
    [HttpGet("forgotPassword")]
    public async Task<ActionResult> ForgotPassword([FromQuery] string email)
    {
        var result = await _authService.SendPasswordResetCode(email);
        if (!result)
            return NotFound("User with this email does not exist.");
   
        return Ok("Password reset code sent to email.");
    }
   
    [HttpPost("resetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var result = await _authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.Code, resetPasswordDto.NewPassword);
        if (!result)
            return BadRequest("Invalid code or email.");
   
        return Ok("Password has been reset successfully.");
    }
}