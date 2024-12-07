using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTeacherManagment.Core.Models;

public class Student : User
{
    /// <summary>
    ///  When student enrolled on a course
    /// </summary>
    public DateTime EnrolledAt { get; set; }

    [ForeignKey(nameof(Group))]
    public Guid? GroupId { get; set; }

    public virtual Group? Group { get; set; }
}