using Application.CQRS.chat_messages.Handlers;
using Application.Security;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using static Application.CQRS.chat_messages.Handlers.SaveChatMessage;

namespace FitnessProject.API.Hubs;

public class ChatHub : Hub
{
    private readonly ISender _sender;
    private readonly IUserContext _userContext;

    public ChatHub(ISender sender, IUserContext userContext)
    {
        _sender = sender;
        _userContext = userContext;
    }

    public async Task SendMessage(int receiverId, string message)
    {
        var senderId = _userContext.MustGetUserId();

        var command = new AddChatMessage.AddChatMessageCommand
        {
            ReceiverId = receiverId,
            Message = message
        };

        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", new
            {

                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            });
        }
        else
        {
            Console.WriteLine("Mesaj gönderilemedi.");
        }
    }
}