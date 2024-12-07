using StudentTeacherManagment.Core.DTOs;

namespace StudentTeacherManagment.Core.Interfaces;

public interface IStudentService
{
    Task<ICollection<StudentDto>> GetStudents(int skip, int take, CancellationToken cancellationToken = default);
    Task<StudentDto?> GetStudentById(Guid id, CancellationToken cancellationToken = default);

    Task<StudentDto> AddStudent(CreateStudentDto student, CancellationToken cancellationToken = default);
    Task<StudentDto> UpdateStudent(Guid id, UpdateStudentDto student, CancellationToken cancellationToken = default);
    Task<StudentDto> DeleteStudent(Guid id, CancellationToken cancellationToken = default);
}