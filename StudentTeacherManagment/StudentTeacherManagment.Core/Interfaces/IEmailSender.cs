namespace StudentTeacherManagment.Core.Interfaces;

public interface IEmailSender
{
    Task Send(string empty);
}