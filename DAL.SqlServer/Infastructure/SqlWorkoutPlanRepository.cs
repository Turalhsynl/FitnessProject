using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlWorkoutPlanRepository(AppDbContext context) : IWorkoutRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(WorkoutPlan plan)
    {
        await _context.WorkoutPlans.AddAsync(plan);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(int userId)
    {
        return await _context.WorkoutPlans
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}
