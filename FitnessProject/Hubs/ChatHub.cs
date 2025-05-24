using Application.Abstractions;
using Application.CQRS.chat_messages.Handlers;
using Application.Services;
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
    private readonly IOpenAIService _openAIService;
    public ChatHub(ISender sender, IOpenAIService openAIService)
    {
        _sender = sender;
        _openAIService = openAIService;
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
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (!int.TryParse(senderIdStr, out var senderId))
        {
            throw new HubException("Invalid sender");
        }

        const int AI_USER_ID = -1;

        //if (receiverId == AI_USER_ID)
        //{
        //    string aiPrompt = message;

        //    if (message.Trim().ToLower() == "salam" && !string.IsNullOrEmpty(userName))
        //    {
        //        aiPrompt = $"Istifadeci sene 'salam' dedi. Onun adı {userName}. Ona adıyla mehriban bir şekilde cavab ver.";
        //    }

        //    var aiResponse = await _openAIService.GetResponseAsync(aiPrompt);

        //    var aiMessageDto = new
        //    {
        //        SenderId = AI_USER_ID,
        //        Message = aiResponse,
        //        SentAt = DateTime.UtcNow
        //    };

        //    await Clients.Caller.SendAsync("ReceiveMessage", aiMessageDto);
        //}
        if (receiverId == AI_USER_ID)
        {
            string aiPrompt = message;

            if (message.Trim().ToLower() == "salam" && !string.IsNullOrEmpty(userName))
            {
                aiPrompt = $"Istifadeci sene 'salam' dedi. Onun adı {userName}. Ona adıyla mehriban bir şekilde cavab ver.";
            }

            var aiResponse = await _openAIService.GetResponseAsync(aiPrompt);

            // Göndərilən mesajı istifadəçiyə göstər
            var userMessageDto = new
            {
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            await Clients.Caller.SendAsync("ReceiveMessage", userMessageDto);

            // AI cavabını göstər
            var aiMessageDto = new
            {
                SenderId = -1,
                Message = aiResponse,
                SentAt = DateTime.UtcNow
            };
            await Clients.Caller.SendAsync("ReceiveMessage", aiMessageDto);
        }
        else
        {
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

                if (_connections.TryGetValue(receiverId.ToString(), out var receiverConnectionId))
                {
                    await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", messageDto);
                }
                else
                {
                    Console.WriteLine("Receiver is not connected.");
                }

                await Clients.Caller.SendAsync("ReceiveMessage", messageDto);
            }
            else
            {
                Console.WriteLine("Message could not be sent.");
            }
        }
    }


}
