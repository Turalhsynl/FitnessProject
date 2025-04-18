using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.SqlServer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartLine> CartLines { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Recipe> Recipes { get; set; }

    public DbSet<FitnessProgram> FitnessPrograms { get; set; }
    public DbSet<FitnessProgramRecipe> FitnessProgramRecipes { get; set; }
    public DbSet<UserProgram> UserPrograms { get; set; }
    public DbSet<MembershipPlan> MembershipPlans { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrdersLines { get; set;}


}
