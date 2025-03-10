using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlUserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Id == id))!;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))!;
    }

    public async Task RegisterAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        user.IsDeleted = true;
        user.DeletedDate = DateTime.Now;
        user.DeletedBy = 0;
    }

    public void Update(User user)
    {
        user.UpdatedDate = DateTime.Now;
        _context.Update(user);
        _context.SaveChanges();
    }
}
