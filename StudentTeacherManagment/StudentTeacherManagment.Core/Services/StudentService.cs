using AutoMapper;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;
using Microsoft.EntityFrameworkCore;
using StudentTeacherManagment.Core.DTOs;

namespace StudentTeacherManagment.Core.Services
{
    public class StudentService : IStudentService
    {
        private const int MinStudentAgeInYears = 14;
        
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public StudentService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<ICollection<StudentDto>> GetStudents(int skip, int take, CancellationToken cancellationToken = default)
        {
            var studentsQuery = _repository.GetAll<Student>().AsNoTracking();
            
            var groups = await studentsQuery.OrderBy(s => s.LastName)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken);

            return _mapper.Map<ICollection<StudentDto>>(groups);
        }

        public async Task<StudentDto?> GetStudentById(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await _repository.GetAll<Student>()
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            
            return _mapper.Map<StudentDto>(group);
        }

        public async Task<StudentDto> AddStudent(CreateStudentDto newStudent, CancellationToken cancellationToken = default)
        {
            var student = _mapper.Map<Student>(newStudent);
            ValidateStudent(student);
            
            // Add student to the repository
            await _repository.AddAsync(student, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentDto>(student);
        }

        private void ValidateStudent(Student student)
        {
            if (string.IsNullOrEmpty(student.FirstName))
            {
                throw new ArgumentException("FirstName must have value", nameof(student.FirstName));
            }
            if (string.IsNullOrEmpty(student.LastName))
            {
                throw new ArgumentException("LastName must have value", nameof(student.LastName));
            }
            if (student.DateOfBirth >= DateTime.Now.AddYears(-MinStudentAgeInYears))
            {
                throw new ArgumentException("Date of birth must be greater than 14 years ago", nameof(student.DateOfBirth));
            }
        }

        public async Task<StudentDto> UpdateStudent(Guid id, UpdateStudentDto student, CancellationToken cancellationToken = default)
        {
            var existingStudent = await _repository.GetAll<Student>()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (existingStudent == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found.");
            }

            _mapper.Map(student, existingStudent);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentDto>(existingStudent);
        }

        public async Task<StudentDto> DeleteStudent(Guid id, CancellationToken cancellationToken = default)
        {
            var student = await _repository.GetAll<Student>()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found.");
            }

            _repository.Delete(student);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentDto>(student);
        }
    }
}
