using DAL.SqlServer.Context;
using DAL.SqlServer.Infastructure;
using DAL.SqlServer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Common;
using Repository.Repositories;

namespace DAL.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServices(this IServiceCollection services, string connectionstring)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionstring));

        services.AddScoped<IUnitOfWork, SqlUnitOfWork>(opt =>
        {
            var dbContext = opt.GetRequiredService<AppDbContext>();
            return new SqlUnitOfWork(connectionstring, dbContext);
        });
        services.AddScoped<IFavoriteRepository, SqlFavoriteRepository>();
        return services;
    }
}
