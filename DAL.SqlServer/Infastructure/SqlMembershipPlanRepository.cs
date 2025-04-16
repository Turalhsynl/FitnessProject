using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlMembershipPlanRepository(AppDbContext context):IMembershipPlanRepository
{
    private readonly AppDbContext _context = context;

    public async Task<MembershipPlan> GetByIdAsync(int id)
    {
        return await _context.MembershipPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && !mp.IsDeleted);
    }

    public async Task<IEnumerable<MembershipPlan>> GetAllAsync()
    {
        return await _context.MembershipPlans
            .Where(mp => !mp.IsDeleted)
            .ToListAsync();
    }

    public async Task AddAsync(MembershipPlan membershipPlan)
    {
        await _context.MembershipPlans.AddAsync(membershipPlan);
    }

    public void Update(MembershipPlan membershipPlan)
    {
        _context.MembershipPlans.Update(membershipPlan);
    }

    public void Remove(MembershipPlan membershipPlan)
    {
        membershipPlan.IsDeleted = true;
        membershipPlan.DeletedDate = DateTime.UtcNow;
        _context.MembershipPlans.Update(membershipPlan);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.MembershipPlans
            .AnyAsync(mp => mp.Id == id && !mp.IsDeleted);
    }
}
