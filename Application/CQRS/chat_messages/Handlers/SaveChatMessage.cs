using Domain.Entities;
using Repository.Repositories;

namespace Application.CQRS.chat_messages.Handlers;

public class SaveChatMessage
{
    public class SaveChatMessageCommand
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
    }

    public class SaveChatMessageHandler
    {
        private readonly IChatMessageRepository _repository;

        public SaveChatMessageHandler(IChatMessageRepository repository)
        {
            _repository = repository;
        }

        public void Handle(SaveChatMessageCommand command)
        {
            var chat = new ChatMessage
            {
                SenderId = command.SenderId,
                ReceiverId = command.ReceiverId,
                Message = command.Message,
                SentAt = DateTime.UtcNow
            };

            _repository.AddAsync(chat);
        }
    }
}
