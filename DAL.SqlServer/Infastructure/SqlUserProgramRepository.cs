using DAL.SqlServer.Context;
using Domain.Entities;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlUserProgramRepository(AppDbContext context):IUserProgramRepository
{
    private readonly AppDbContext _context = context;

    public void AddUserProgram(UserProgram userProgram)
    {
        _context.UserPrograms.Add(userProgram);
    }

    public bool Exists(int userId, int programId)
    {
        return _context.UserPrograms
            .Any(up => up.UserId == userId && up.ProgramId == programId);
    }

    public IEnumerable<UserProgram> GetAllUserPrograms()
    {
        return _context.UserPrograms.ToList();
    }

    public IEnumerable<FitnessProgram> GetProgramsByUserId(int userId)
    {
        return _context.UserPrograms
            .Where(up => up.UserId == userId)
            .Select(up => up.Program)
            .ToList();
    }

    public UserProgram GetUserProgram(int userId, int programId)
    {
        return _context.UserPrograms
           .FirstOrDefault(up => up.UserId == userId && up.ProgramId == programId);
    }

    public void RemoveUserProgram(int userId, int programId)
    {
        var entity = _context.UserPrograms
            .FirstOrDefault(up => up.UserId == userId && up.ProgramId == programId);
        if (entity != null)
        {
            _context.UserPrograms.Remove(entity);
        }
    }
}
