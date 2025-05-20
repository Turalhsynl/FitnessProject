using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Auth.GoogleLoginCommandHandler;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoogleAuthController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        var result = await _sender.Send(new GoogleLoginCommand { Code = code });
        return Ok(result);
    }
}
