using Domain.Entities;

namespace Repository.Repositories;

public interface IFileUploadRepository
{
    Task AddAsync(Domain.Entities.File file);
    Task Remove(int id);
    Task<Domain.Entities.File> GetByIdAsync(int id);
    Task<Domain.Entities.File> GetByNameAsync(string name);
}
