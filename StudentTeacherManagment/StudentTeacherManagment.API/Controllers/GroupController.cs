using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentTeacherManagment.Core.DTOs;
using StudentTeacherManagment.Core.Interfaces;
using StudentTeacherManagment.Core.Models;

namespace StudentTeacherManagment.API.Controllers;

[ApiController]
[Route("groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;
    
    public GroupController(IGroupService groupService, IMapper mapper)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups(
        [FromQuery] string? name = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
    {
        var groups = await _groupService.GetGroups(name, skip, take);
        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
    }
    
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GroupDto>> GetGroupById(Guid id)
    {
        var group = await _groupService.GetGroupById(id);

        if (group == null)
        {
            return NotFound($"Group with ID {id} not found.");
        }

        return Ok(_mapper.Map<GroupDto>(group));
    }
    
    [Authorize(Roles = "Teacher")] 
    [HttpPost]
    public async Task<ActionResult<GroupDto>> AddGroup([FromBody] CreateGroupDto groupCreateDto)
    {
        var createdGroup = await _groupService.AddGroup(groupCreateDto);

        return CreatedAtAction(nameof(GetGroupById), new { id = createdGroup.Id }, _mapper.Map<GroupDto>(createdGroup));
    }
    
    [Authorize(Roles = "Teacher")] 
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        try
        {
            await _groupService.DeleteGroup(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [Authorize(Roles = "Teacher")] 
    [HttpPost("{groupId:guid}/students/{studentId:guid}")]
    public async Task<IActionResult> AddStudentToGroup(Guid groupId, Guid studentId)
    {
        try
        {
            await _groupService.AddStudentToGroup(groupId, studentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}