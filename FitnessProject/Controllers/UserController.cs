using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{   
    private readonly ISender _sender = sender;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] Application.CQRS.Users.Handlers.Register.Command request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Application.CQRS.Users.Handlers.GetById.Query request)
    {
        return Ok(await _sender.Send(request));
    }

}
