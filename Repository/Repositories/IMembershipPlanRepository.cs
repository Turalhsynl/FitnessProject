using Domain.Entities;

namespace Repository.Repositories;

public interface IMembershipPlanRepository
{
    Task<MembershipPlan> GetByIdAsync(int id);//
    Task<IEnumerable<MembershipPlan>> GetAllAsync();//
    Task AddAsync(MembershipPlan membershipPlan);//
    void Update(MembershipPlan membershipPlan);//
    void Remove(MembershipPlan membershipPlan);//
    Task<bool> ExistsAsync(int id);
}
