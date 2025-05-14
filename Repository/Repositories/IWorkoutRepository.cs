using Domain.Entities;

namespace Repository.Repositories;

public interface IWorkoutRepository
{
    Task AddAsync(WorkoutPlan plan);
    Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(int userId);
}
