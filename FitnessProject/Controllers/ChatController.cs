using Application.CQRS.chat_messages.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] AddChatMessage.AddChatMessageCommand command)
    {
        var result = await _sender.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("conversation")]
    public async Task<IActionResult> GetConversation([FromQuery] int user1Id, [FromQuery] int user2Id)
    {
        var query = new GetConversation.GetConversationQuery
        {
            User1Id = user1Id,
            User2Id = user2Id
        };

        var result = await _sender.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
