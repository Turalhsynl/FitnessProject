using Application.CQRS.UserPrograms.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.UserProgram.Handlers.AddUserProgram;
using static Application.CQRS.UserPrograms.Handlers.GetAllUserPrograms;
using static Application.CQRS.UserPrograms.Handlers.GetUserProgramsByUserId;
using static Application.CQRS.UserPrograms.Handlers.RemoveUserProgram;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProgramController : ControllerBase
{
    private readonly ISender _sender;

    public UserProgramController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] AddUserProgramCommand command)
    {
        var result = await _sender.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }
    [HttpDelete]
    public async Task<IActionResult> RemoveUserProgramAsync([FromQuery] int userId, [FromQuery] int programId)
    {
        var command = new RemoveUserProgram.RemoveUserProgramCommand
        {
            UserId = userId,
            ProgramId = programId
        };

        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetAllUserProgramsQuery();
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet("programs-by-user")]
    public async Task<IActionResult> GetProgramsByUser([FromQuery] int userId)
    {
        var query = new GetUserProgramsByUserId.GetUserProgramsByUserIdQuery
        {
            UserId = userId
        };

        var result = await _sender.Send(query);
        return Ok(result);
    }

    //[HttpGet("exists")]
    //public async Task<IActionResult> Exists([FromQuery] int userId, [FromQuery] int programId)
    //{
    //    var query = new CheckUserProgramExistsQuery(userId, programId);
    //    var result = await _sender.Send(query);
    //    return Ok(result);
    //}

    //[HttpGet("get")]
    //public async Task<IActionResult> GetAsync([FromQuery] int userId, [FromQuery] int programId)
    //{
    //    var query = new GetUserProgramQuery(userId, programId);
    //    var result = await _sender.Send(query);
    //    if (!result.IsSuccess)
    //        return NotFound(result.Errors);

    //    return Ok(result.Data);
    //}
}
