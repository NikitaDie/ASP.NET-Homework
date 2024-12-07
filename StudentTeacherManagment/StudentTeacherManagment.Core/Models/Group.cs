namespace StudentTeacherManagment.Core.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Student> Students { get; set; }
}