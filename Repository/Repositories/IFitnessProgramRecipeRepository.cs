using Domain.Entities;

namespace Repository.Repositories;

public interface IFitnessProgramRecipeRepository
{
    Task AddAsync(FitnessProgramRecipe entity);
    Task RemoveAsync(FitnessProgramRecipe entity);
    Task<FitnessProgramRecipe> GetByProgramAndRecipeAsync(int programId, int recipeId);
    Task<bool> ExistsAsync(int programId, int recipeId);
    Task<List<Recipe>> GetRecipesByFitnessProgramIdAsync(int fitnessProgramId);

}
