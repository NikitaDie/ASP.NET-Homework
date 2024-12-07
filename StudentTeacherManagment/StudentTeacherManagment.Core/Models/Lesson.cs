namespace StudentTeacherManagment.Core.Models;

public class Lesson
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public virtual Teacher Teacher { get; set; }
    public virtual Subject Subject { get; set; }
    public virtual Group Group { get; set; }
}


/*
 *
 * Group:
 * - create
 * - delete
 * - add student to group
 * 
 * Student:
 * - create
 * - delete
 * - update
 * 
 * Teacher:
 * - create teacher
 * - delete
 * - update
 * - add new subject
 * 
 * Lesson:
 * - create
 * - delete
 * - update
 *
 * Auth
 * - login
 * - resetPassword
 *
 * Subject:
 * - create
 * - delete
 * - update
 * - create schedule
 */