using Application.CQRS.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{   
    private readonly ISender _sender = sender;

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] Application.CQRS.Users.Handlers.Register.RegisterCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("ById")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Application.CQRS.Users.Handlers.GetById.Query request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("ByEmail")]
    public async Task<IActionResult> GetByEmailAsync([FromQuery] Application.CQRS.Users.Handlers.GetByEmail.EmailQuery request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync([FromBody] Application.CQRS.Users.Handlers.Update.UpdateCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var request = new Application.CQRS.Users.Handlers.Delete.DeleteCommand { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _sender.Send(new GetAll.GetAllUsersQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }
}
