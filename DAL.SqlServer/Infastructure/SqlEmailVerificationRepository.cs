using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlEmailVerificationRepository(AppDbContext context) : IEmailVerificationRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(EmailVerification entity)
    {
        await _context.EmailVerifications.AddAsync(entity);
    }

    public async Task<EmailVerification?> GetValidCodeAsync(string email, string code)
    {
        return await _context.EmailVerifications
            .Where(x => x.Email == email && x.Code == code && !x.IsUsed && x.ExpireAt > DateTime.UtcNow)
            .OrderByDescending(x => x.ExpireAt)
            .FirstOrDefaultAsync();
    }
}

