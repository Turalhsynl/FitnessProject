using Domain.Entities;

namespace Repository.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    void Update(Category category);
    Task<bool> Delete(int id, int deletedBy);
    IQueryable<Category> GetAll();
    Task<Category> GetByIdAsync(int id);
}
