using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlRecipeRepository(AppDbContext context) : IRecipeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var recipe = await GetByIdAsync(id);
        if (recipe == null) 
        {
            return false;
        
        }
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes.ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> GetByCalorieRangeAsync(int minCalories, int maxCalories)
    {
        return await _context.Recipes.Where(r=>r.Calories >= minCalories && r.Calories<=maxCalories).ToListAsync();
    }

    public async Task<Recipe> GetByIdAsync(int id)
    {
        return await _context.Recipes.FindAsync(id);
    }

    public async Task<IEnumerable<Recipe>> GetByIngredientAsync(string ingredient)
    {
        return await _context.Recipes.Where(r=> r.Ingredients.Contains(ingredient)).ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> GetByMealTypeAsync(string mealType)
    {
        return await _context.Recipes.Where(r=> r.MealType == mealType).ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> SearchByNameAsync(string name)
    {
        name = name.ToLower();

        return await _context.Recipes
            .Where(r => r.Name.ToLower().Contains(name) || r.Description.ToLower().Contains(name))
            .ToListAsync();
    }



    public async Task<Recipe> UpdateAsync(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }
}
