using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlFitnessProgramRecipeRepository(AppDbContext context) : IFitnessProgramRecipeRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(FitnessProgramRecipe entity)
    {
        await _context.FitnessProgramRecipes.AddAsync(entity);
    }

    public async Task RemoveAsync(FitnessProgramRecipe entity)
    {
        _context.FitnessProgramRecipes.Remove(entity);
    }

    public async Task<FitnessProgramRecipe> GetByProgramAndRecipeAsync(int programId, int recipeId)
    {
        return await _context.FitnessProgramRecipes
            .FirstOrDefaultAsync(x => x.FitnessProgramId == programId && x.RecipeId == recipeId && !x.IsDeleted);
    }

    public async Task<bool> ExistsAsync(int programId, int recipeId)
    {
        return await _context.FitnessProgramRecipes
            .AnyAsync(x => x.FitnessProgramId == programId && x.RecipeId == recipeId && !x.IsDeleted);
    }

    public async Task<List<Recipe>> GetRecipesByFitnessProgramIdAsync(int fitnessProgramId)
    {
        return await _context.FitnessProgramRecipes
            .Where(fpr => fpr.FitnessProgramId == fitnessProgramId && !fpr.IsDeleted)
            .Include(fpr => fpr.Recipe)
            .Select(fpr => fpr.Recipe)
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }

}
