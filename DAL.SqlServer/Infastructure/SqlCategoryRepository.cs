using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlCategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id, int deletedBy)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) { return false; }

        category.IsDeleted = true;
        category.DeletedBy = deletedBy;
        category.DeletedDate = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public IQueryable<Category> GetAll()
    {
        return _context.Categories;
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return (await _context.Categories.FirstOrDefaultAsync(c => c.Id == id))!;
    }

    public void Update(Category category)
    {
        category.UpdatedDate = DateTime.Now;
        _context.Update(category);
        _context.SaveChanges();
    }
}
