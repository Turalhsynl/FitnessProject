using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using static Application.CQRS.chat_messages.Handlers.SaveChatMessage;

namespace FitnessProject.API.Hubs;

public class ChatHub : Hub
{
    private readonly SaveChatMessageHandler _handler;
    private static readonly ConcurrentDictionary<string, int> _connections = new();

    public ChatHub(SaveChatMessageHandler handler)
    {
        _handler = handler;
    }

    public override Task OnConnectedAsync()
    {
        var userIdString = Context.GetHttpContext().Request.Query["userId"];
        if (int.TryParse(userIdString, out int userId))
        {
            _connections[Context.ConnectionId] = userId;
        }
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connections.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendPrivateMessage(int senderId, int receiverId, string message)
    {
        _handler.Handle(new SaveChatMessageCommand
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message
        });

        var connectionId = _connections.FirstOrDefault(x => x.Value == receiverId).Key;

        if (connectionId != null)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
