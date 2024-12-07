using AutoMapper;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;
using Microsoft.EntityFrameworkCore;
using StudentTeacherManagment.Core.DTOs;

namespace StudentTeacherManagment.Core.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GroupService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region DQL
        
        public async Task<ICollection<GroupDto>> GetGroups(string? name, int skip, int take, CancellationToken cancellationToken = default)
        {
            var groupsQuery = _repository.GetAll<Group>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                groupsQuery = groupsQuery.Where(g => g.Name.Contains(name));
            }
            
             var groups = await groupsQuery.OrderBy(g => g.Name)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken);

             return _mapper.Map<ICollection<GroupDto>>(groups);
        }

        public async Task<GroupDto?> GetGroupById(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await _repository.GetAll<Group>()
                .Include(g => g.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            
            return _mapper.Map<GroupDto>(group);
        }
        
        #endregion

        #region DML

        public async Task<GroupDto> AddGroup(CreateGroupDto newGroup, CancellationToken cancellationToken = default)
        {
            var group = _mapper.Map<Group>(newGroup);
            await _repository.AddAsync(_mapper.Map<Group>(group), cancellationToken);
            await _repository.SaveChangesAsync();

            return _mapper.Map<GroupDto>(group);
        }

        public async Task DeleteGroup(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await _repository.GetAll<Group>()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {id} not found.");
            }

            _repository.Delete(group);
            await _repository.SaveChangesAsync();
        }

        public async Task AddStudentToGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken = default)
        {
            var group = await _repository.GetAll<Group>()
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }

            var student = await _repository.GetAll<Student>()
                .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {studentId} not found.");
            }

            group.Students.Add(student);
            await _repository.SaveChangesAsync();
        }

        #endregion
    }
}
