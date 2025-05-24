using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Auth.GoogleLoginCommandHandler;
using static Application.CQRS.EmailVerification.Handlers.EmailVerificationHandler;
using static Application.CQRS.EmailVerification.Handlers.VerifyEmailHandler;

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

    [HttpPost("send-email-code")]
    public async Task<IActionResult> SendEmailCode([FromBody] GenerateEmailCodeCommand command)
    {
        await _sender.Send(command);
        return Ok("Kod göndərildi");
    }

    [HttpPost("verify-email-code")]
    public async Task<IActionResult> VerifyEmailCode([FromBody] VerifyEmailCodeCommand command)
    {
        var result = await _sender.Send(command);
        if (!result) return BadRequest("Kod yanlışdır və ya vaxtı bitib");

        return Ok("Email təsdiqləndi");
    }
}
