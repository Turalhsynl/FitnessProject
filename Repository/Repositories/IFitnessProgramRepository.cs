namespace Repository.Repositories;

public interface IFitnessProgramRepository
{
    Task<FitnessProgram> GetByIdAsync(int id);
    Task<IEnumerable<FitnessProgram>> GetAllAsync();
    Task AddAsync(FitnessProgram program);
    void Update(FitnessProgram program);
    Task SoftDeleteAsync(int id);

    //Task<FitnessProgram> GetWithRecipesAsync(int id);
}
