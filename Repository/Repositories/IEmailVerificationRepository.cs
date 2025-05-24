using Domain.Entities;

namespace Repository.Repositories;

public interface IEmailVerificationRepository
{
    Task<EmailVerification?> GetValidCodeAsync(string email, string code);
    Task AddAsync(EmailVerification entity);
}