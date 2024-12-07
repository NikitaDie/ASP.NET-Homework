using AutoMapper;
using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Models;
using Group = System.Text.RegularExpressions.Group;

namespace StudentTeacherManagment.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<Group, GroupDto>();
        CreateMap<CreateGroupDto, Group>();
        CreateMap<User, UserDto>();
    }
}