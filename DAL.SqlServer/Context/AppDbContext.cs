using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.SqlServer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
