using StudentTeacherManagment.Core.Interfaces;

namespace StudentTeacherManagment.Core.Fakes;

public class FakeEmailSender : IEmailSender
{
    public async Task Send(string message)
    {
        Console.WriteLine("*** FakeEmail sender: " + message + " ***");
    }
}