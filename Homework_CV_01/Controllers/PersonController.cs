using Homework_CV_01.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework_CV_01.Controllers;

public class PersonController : Controller
{
    // GET
    public IActionResult Index()
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 30,
            Profession = "Software Developer",
            Summary = "An experienced developer with a passion for coding and learning new technologies.",
            PhotoUrl = "/images/john_doe.jpg",
            Softskills = new List<string> { "Communication", "Teamwork", "Problem-solving" },
            Techskills = new List<string> { "C#", "ASP.NET Core", "SQL", "JavaScript" },
            SocialMedia = new Dictionary<string, string>{
                { "GitHub", "https://github.com/your-github-profile" },
                { "LinkedIn", "https://www.linkedin.com/in/your-linkedin-profile" }
            },
        };
        
        return View(person);
    }
}