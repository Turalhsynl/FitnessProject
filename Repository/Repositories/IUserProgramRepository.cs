using Domain.Entities;

namespace Repository.Repositories;

public interface IUserProgramRepository
{
    void AddUserProgram(UserProgram userProgram);//
    void RemoveUserProgram(int userId, int programId);//
    bool Exists(int userId, int programId);
    IEnumerable<FitnessProgram> GetProgramsByUserId(int userId);//
    UserProgram GetUserProgram(int userId, int programId);
    IEnumerable<UserProgram> GetAllUserPrograms();//
}
