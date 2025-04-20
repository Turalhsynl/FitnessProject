using Application.CQRS.chat_messages.Handlers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using static Application.CQRS.chat_messages.Handlers.SaveChatMessage;

namespace FitnessProject.API.Hubs;

public class ChatHub : Hub
{
    private readonly ISender _sender;

    private static readonly ConcurrentDictionary<string, string> _connections = new();

    public ChatHub(ISender sender)
    {
        _sender = sender;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User must login.");
        }

        _connections.TryAdd(userId, Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            _connections.TryRemove(userId, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(int receiverId, string message)
    {
        var senderIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(senderIdStr, out var senderId))
        {
            throw new HubException("Invalid sender");
        }

        var command = new AddChatMessage.AddChatMessageCommand
        {
            ReceiverId = receiverId,
            Message = message
        };

        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            var messageDto = new
            {
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            // Qarşı tərəfə göndər
            if (_connections.TryGetValue(receiverId.ToString(), out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", messageDto);
            }
            else
            {
                Console.WriteLine("Receiver is not connected.");
            }

            // Göndərən tərəfə də göndər (sənin problemi həll edən hissə)
            await Clients.Caller.SendAsync("ReceiveMessage", messageDto);
        }
        else
        {
            Console.WriteLine("Message could not be sent.");
        }
    }

}
