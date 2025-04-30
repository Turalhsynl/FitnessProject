using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlChatMessageRepository : IChatMessageRepository
{
    private readonly AppDbContext _context;

    public SqlChatMessageRepository(AppDbContext context)
    {
        _context = context;
    }

  

    public async Task AddAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<ChatMessage>> GetConversationAsync(int user1Id, int user2Id)
    {
        return await _context.ChatMessages
            .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}
