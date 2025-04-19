using Domain.Entities;

namespace Repository.Repositories;

public interface IChatMessageRepository
{
  
   
        Task AddAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetConversationAsync(int user1Id, int user2Id);
    

}
