using Domain.Entities;

namespace Repository.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> AddAsync (Recipe recipe);//
    Task<Recipe> GetByIdAsync (int id);//

    Task<IEnumerable<Recipe>> GetAllAsync ();//
    Task<Recipe> UpdateAsync (Recipe recipe);//
    Task<bool> DeleteAsync (int id);//
    Task<IEnumerable<Recipe>> GetByMealTypeAsync (string mealType);//
    Task<IEnumerable<Recipe>> SearchByNameAsync (string name);//
    Task<IEnumerable<Recipe>> GetByCalorieRangeAsync(int minCalories, int maxCalories);//
    Task<IEnumerable<Recipe>> GetByIngredientAsync(string ingredient);//
}
