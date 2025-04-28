using DAL.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlFitnessProgramRepository(AppDbContext context) : IFitnessProgramRepository
{
    private readonly AppDbContext _context = context;

    public async Task<FitnessProgram> GetByIdAsync(int id)
    {
        return await _context.FitnessPrograms
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<IEnumerable<FitnessProgram>> GetAllAsync()
    {
        return await _context.FitnessPrograms
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<FitnessProgram>> GetByUserIdAsync(int userId)
    {
        return await _context.FitnessPrograms
                             .Where(fp => fp.UserId == userId)
                             .ToListAsync();
    }


    public async Task AddAsync(FitnessProgram program)
    {
        await _context.FitnessPrograms.AddAsync(program);
    }

    public void Update(FitnessProgram program)
    {
        _context.FitnessPrograms.Update(program);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var program = await _context.FitnessPrograms.FindAsync(id);
        if (program is not null)
        {
            program.IsDeleted = true;
            program.DeletedDate = DateTime.UtcNow;
        }
    }

    //public async Task<FitnessProgram> GetWithRecipesAsync(int id)
    //{
    //    return await _context.FitnessPrograms
    //        .Include(x => x.FitnessProgramRecipes)
    //            .ThenInclude(fpr => fpr.Recipe)
    //        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    //}
}
