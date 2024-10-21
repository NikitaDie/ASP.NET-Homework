namespace Homework_CV_01.Models;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Profession { get; set; }
    public string Summary { get; set; }
    public string PhotoUrl { get; set; }
    public Dictionary<string, string> SocialMedia { get; set; }
    public List<string> Softskills { get; set; }
    public List<string> Techskills { get; set; }
}