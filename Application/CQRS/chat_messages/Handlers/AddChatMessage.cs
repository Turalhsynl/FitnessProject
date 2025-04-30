using Application.Security;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.chat_messages.Handlers;

public class AddChatMessage
{
    public class AddChatMessageCommand : IRequest<Result<string>>
    {
        public int ReceiverId { get; set; }
        public string Message { get; set; }
    }

    public sealed class AddChatHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
        : IRequestHandler<AddChatMessageCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<string>> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
        {
            var senderId = _userContext.MustGetUserId();

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Message = request.Message,
                SentAt = DateTime.UtcNow
            };

            await _unitOfWork.ChatMessageRepository.AddAsync(chatMessage);
            await _unitOfWork.SaveChangeAsync();

            return new Result<string>
            {
                IsSuccess = true,
                Data = "Message sent successfully"
            };
        }
    }
}
