using Application.Abstractions;
using Application.CQRS.chat_messages.Handlers;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(ISender sender, IOpenAIService openAIService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly IOpenAIService _openAIService= openAIService;

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] AddChatMessage.AddChatMessageCommand command)
    {
        var result = await _sender.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpPost("ask-ai")]
    public async Task<IActionResult> AskAI([FromBody] string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
        {
            return BadRequest("Message cannot be empty.");
        }

        var aiResponse = await _openAIService.GetResponseAsync(userMessage);

        return Ok(new { Response = aiResponse });
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
