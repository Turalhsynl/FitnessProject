using DAL.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.Helpers;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlFileUploadRepository(AppDbContext context) : IFileUploadRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Domain.Entities.File file)
    {
        await _context.Files.AddAsync(file);
    }

    public async Task<Domain.Entities.File> GetByIdAsync(int id)
    {
        return await _context.Files
              .FirstOrDefaultAsync(x => x.Id == id &&  x.IsDeleted == false);
    }

    public Task<Domain.Entities.File> GetByNameAsync(string name)
    {
        return _context.Files.Where(x => x.FileName == name).FirstOrDefaultAsync();

    }

    public async Task Remove(int id)
    {
        var currentFile = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
        currentFile.IsDeleted = true;
        currentFile.DeletedDate = DateTime.UtcNow;
        _context.Files.Update(currentFile);
    }
}
