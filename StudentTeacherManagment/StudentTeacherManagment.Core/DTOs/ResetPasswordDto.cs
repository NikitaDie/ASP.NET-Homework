namespace StudentTeacherManagment.Core.DTOs;

public class ResetPasswordDto
{
    public string Email { get; set; }
    public int Code { get; set; }
    public string NewPassword { get; set; }
}