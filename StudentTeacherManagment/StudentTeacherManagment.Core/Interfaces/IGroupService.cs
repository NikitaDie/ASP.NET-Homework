using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Models;

namespace StudentTeacherManagment.Core.Interfaces;

public interface IGroupService
{
    // DQL - Data Query Language
    Task<ICollection<GroupDto>> GetGroups(string? name, int skip, int take, CancellationToken cancellationToken = default);
    Task<GroupDto?> GetGroupById(Guid id, CancellationToken cancellationToken = default);
    
    // DML - Data Manipulation Language
    Task<GroupDto> AddGroup(CreateGroupDto group, CancellationToken cancellationToken = default);
    Task DeleteGroup(Guid id, CancellationToken cancellationToken = default);
    Task AddStudentToGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken = default);
    
    // DDL = Data Definition Language
    // TCL - Transaction Control Language
}