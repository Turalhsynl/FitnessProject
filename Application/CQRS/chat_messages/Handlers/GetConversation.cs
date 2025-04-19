using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.chat_messages.Handlers;

public class GetConversation
{
    public class GetConversationQuery : IRequest<Result<List<ChatMessage>>>
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetConversationQuery, Result<List<ChatMessage>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<ChatMessage>>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
        {
            var messages = await _unitOfWork.ChatMessageRepository
                .GetConversationAsync(request.User1Id, request.User2Id);

            return new Result<List<ChatMessage>>
            {
                Data = messages.ToList(),
                IsSuccess = true
            };
        }
    }
}
