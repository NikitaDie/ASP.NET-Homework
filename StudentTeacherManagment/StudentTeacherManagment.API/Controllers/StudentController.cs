using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentTeacherManagment.API.Filters;
using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;

namespace StudentTeacherManagment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogRequestFilter))]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/student
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents(
        [FromQuery] int skip = 0, 
        [FromQuery] int take = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var students = await _studentService.GetStudents(skip, take, cancellationToken);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    // GET: api/student/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentService.GetStudentById(id, cancellationToken);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<StudentDto>> AddStudent(
        [FromBody] CreateStudentDto student, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var addedStudent = await _studentService.AddStudent(student, cancellationToken);

            return CreatedAtAction(nameof(GetStudentById), new { id = addedStudent.Id }, addedStudent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    // PUT: api/student/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Student>> UpdateStudent(
        Guid id, 
        [FromBody] UpdateStudentDto student, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var updatedStudent = await _studentService.UpdateStudent(id, student, cancellationToken);
            return Ok(updatedStudent);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/student/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<StudentDto>> DeleteStudent(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var deletedStudent = await _studentService.DeleteStudent(id, cancellationToken);
            return Ok(deletedStudent);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}