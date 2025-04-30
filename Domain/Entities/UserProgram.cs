using Domain.BaseEntities;

namespace Domain.Entities;

public class UserProgram : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int ProgramId { get; set; }
    public FitnessProgram Program { get; set; }
}
